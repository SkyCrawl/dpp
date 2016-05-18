﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Ini.Specification;
using Ini.EventLoggers;
using Ini.Util;
using Ini.Exceptions;
using Ini.Configuration.Base;
using Ini.Configuration.Values;
using System.IO;

namespace Ini.Configuration
{
    /// <summary>
    /// Class representing option as a whole.
    /// </summary>
    public class Option : ConfigBlockBase, IEnumerable<IElement>
    {
        #region Properties

        /// <summary>
        /// Readonly type that must be shared by values of all <see cref="Values"/>.
        /// Should you wish to change the type, it's better to create a whole new
        /// <see cref="Option"/>, perhaps using the same identifier.
        /// </summary>
        public Type ValueType { get; private set; }

        /// <summary>
        /// Trailing commentary for the option.
        /// </summary>
        public string TrailingCommentary { get; set; }

        /// <summary>
        /// The position of the trailing commentary.
        /// </summary>
        public int TrailingCommentaryPosition { get; set; }

        /// <summary>
        /// This option's values. Consumers are given direct access to the collection but they might break some
        /// invariants so the collection is observed internally. When an invalid operation is performed,
        /// an exception is thrown.
        /// <seealso cref="OnValuesChanged"/>
        /// </summary>
        public ObservableList<IElement> Elements { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class.
        /// </summary>
        public Option(string identifier, Type elementType, string commentary = null, int commentaryPosition = 0) : base(identifier)
        {
            ValueType = elementType;
            TrailingCommentary = commentary;
            TrailingCommentaryPosition = commentaryPosition;

            Elements = new ObservableList<IElement>(OnValuesChanged);
        }

        #endregion

        #region Public methods querying content

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <returns>The element, or null if index is out of range.</returns>
        /// <param name="elementIndex">The index.</param>
        public IElement GetElement(int elementIndex)
        {
            try
            {
                return Elements[elementIndex];
            }
            catch(IndexOutOfRangeException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the element at the specified index, correctly typed.
        /// </summary>
        /// <returns>The element, or null if index is out of range.</returns>
        /// <param name="elementIndex">The index.</param>
        /// <exception cref="InvalidCastException">If the specified type was not correct.</exception>
        public T GetElement<T>(int elementIndex) where T : IElement
        {
            return (T) GetElement(elementIndex);
        }

        /// <summary>
        /// Gets the value at the specified index, correctly typed.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="elementIndex">Target element index.</param>
        /// <exception cref="InvalidCastException">The specified type was incorrect.</exception>
        /// <exception cref="IndexOutOfRangeException">If the index is out of range, surprisingly.</exception>
        /// <exception cref="InvalidOperationException">Target element is a link and it contains zero or multiple
        /// values, or it was an instance of <see cref="ValueStub"/>, or it was an unknown subtype of 
        /// <see cref="IElement"/>.</exception>
        public OutputType GetValue<OutputType>(int elementIndex)
        {
            IElement elem = GetElement(elementIndex);
            if(elem == null)
            {
                throw new IndexOutOfRangeException();
            }
            else if(elem is IValue)
            {
                return (elem as IValue).GetValue<OutputType>();
            }
            else if(elem is ILink)
            {
                return (elem as ILink).Values.Single().GetValue<OutputType>();
            }
            else
            {
                throw new InvalidOperationException("Unhandled value type encountered: " + elem.GetType().ToString());
            }
        }

        /// <summary>
        /// Converts this option's elements into a collection of value (<see cref="IValue"/>) objects.
        /// </summary>
        /// <exception cref="ArgumentException">An element's type is not handled in this method.</exception>
        /// <returns>The collection.</returns>
        public IList<IValue> GetObjectValues()
        {
            List<IValue> result = new List<IValue>();
            foreach(IElement elem in Elements)
            {
                if(elem is IValue)
                {
                    result.Add(elem as IValue);
                }
                else if(elem is ILink)
                {
                    result.AddRange((elem as ILink).Values);
                }
                else // simply a general element
                {
                    throw new ArgumentException("Unhandled value type encountered: " + elem.GetType().ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// Converts this option's elements into an array of elementary values.
        /// </summary>
        /// <returns>The array.</returns>
        /// <typeparam name="OutputType">The correct type.</typeparam>
        /// <exception cref="ArgumentException">An element's type is not handled in the underlying method.</exception>
        /// <exception cref="System.InvalidCastException">The specified type was incorrect.</exception>
        public OutputType[] GetValues<OutputType>()
        {
            return GetObjectValues().GetValues<OutputType>();
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether this instance is valid the specified config section specification configLogger.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid the specified config section specification configLogger; otherwise, <c>false</c>.</returns>
        /// <param name="config">The parent configuration.</param>
        /// <param name="section">The parent section identifier.</param>
        /// <param name="specification">The option specification to validate against.</param>
        /// <param name="logger">Configuration validation event logger.</param>
        public bool IsValid(Config config, string section, OptionSpec specification, IConfigValidatorEventLogger logger)
        {
            // prepare the result validation state
            bool result = true;

            // validate general conditions
            int valueCount = GetObjectValues().Count;
            if(specification.IsMandatory && valueCount == 0)
            {
                result = false;
                logger.NoValue(section, Identifier);
            }
            if(specification.HasSingleValue && valueCount > 1)
            {
                result = false;
                logger.TooManyValues(section, Identifier);
            }
            if(!ValueType.Equals(specification.GetValueType()))
            {
                result = false;
                logger.ValueTypeMismatch(
                    section,
                    Identifier,
                    specification.GetValueType(),
                    ValueType);
            }
            else
            {
                // only validate inner elements if there's no type mismatch
                foreach(IElement element in Elements)
                {
                    if(!element.IsValid(config, section, specification, logger))
                    {
                        result = false;
                    }
                }
            }

            // and return
            return result;
        }

        #endregion

        #region Protected interface

        /// <summary>
        /// Observer for <see cref="Values"/> that keeps the integrity of the
        /// collection's items. At the moment, there is only one invariant: every item's
        /// type has to be equal to <see cref="ValueType"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        /// <exception cref="Ini.Exceptions.InvariantBrokenException">An invariant was broken.</exception>
        protected void OnValuesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    // check the invariants and throw an exception if required
                    foreach(IElement elem in e.NewItems)
                    {
                        if(!ValueType.IsAssignableFrom(elem.ValueType))
                        {
                            throw new InvariantBrokenException(string.Format(
                                "Only elements with value type of '{0}' are allowed.",
                                ValueType.FullName));
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                    // one or more items were moved or removed - that doesn't break any invariant
                    break;

                default:
                    throw new ArgumentException("Unknown enum value: " + e.Action.ToString());
            }
        }

        #endregion

        /// <summary>
        /// Serializes this instance into the specified text writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="options">Serialization options.</param>
        /// <param name="sectionSpecification">Section specification of the current configuration block.</param>
        /// <param name="config">The parent configuration.</param>
        internal override void SerializeSelf(TextWriter writer, ConfigWriterOptions options, SectionSpec sectionSpecification, Config config)
        {
            var optionString = IniSyntax.SerializeOption(Identifier, Elements, config);
            var commentarySpacesCount = TrailingCommentaryPosition - optionString.Length;

            writer.Write(optionString);
            writer.WriteLine(IniSyntax.SerializeCommentary(TrailingCommentary, commentarySpacesCount > 1 ? commentarySpacesCount : 1));
        }

        #region IEnumerable implementation

        /// <summary>
        /// Gets the content enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator<IElement> IEnumerable<IElement>.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        /// <summary>
        /// Gets the content enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        #endregion
    }
}
