using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Ini.Specification;
using Ini.Configuration.Elements;
using Ini.EventLogs;
using Ini.Util;
using Ini.Exceptions;
using Ini.Validation;

namespace Ini.Configuration
{
    /// <summary>
    /// Class representing option as a whole.
    /// </summary>
    public class Option : ConfigBlockBase, IEnumerable<IElement>
    {
        #region Properties

        /// <summary>
        /// Readonly type that must be shared by values of all <see cref="Elements"/>.
        /// Should you wish to change the type, it's better to create a whole new
        /// <see cref="Option"/>, perhaps using the same identifier.
        /// </summary>
        public Type ValueType { get; private set; }

        /// <summary>
        /// Trailing commentary for the option.
        /// </summary>
        public string TrailingCommentary { get; set; }

        /// <summary>
        /// This option's value, consisting of subclasses of <see cref="Element{T}"/>.
        /// Consumers are given direct access to the collection but they might break some
        /// invariants so the collection is observed internally. When an invalid operation
        /// is performed, an exception is thrown.
        /// <seealso cref="OnElementsChanged"/>
        /// </summary>
        public ObservableCollection<IElement> Elements { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class.
        /// </summary>
        public Option(string identifier, Type elementType, string commentary = null) : base(identifier)
        {
            this.ValueType = elementType;
            this.TrailingCommentary = commentary;
            this.Elements = new ObservableCollection<IElement>();
            this.Elements.CollectionChanged += OnElementsChanged;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the specified element.
        /// </summary>
        /// <returns>The element, or null if not found.</returns>
        /// <param name="elementIndex">Target element index.</param>
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
        /// Gets the specified element, correctly typed.
        /// </summary>
        /// <returns>The element, or null if not found.</returns>
        /// <param name="elementIndex">Target element index.</param>
        /// <exception cref="InvalidCastException">If the specified type was not correct.</exception>
        public T GetElement<T>(int elementIndex) where T : IElement
        {
            return (T) GetElement(elementIndex);
        }

        /// <summary>
        /// Gets the value of a single, correctly typed element. If the specified type
        /// is not correct, throws an exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="System.InvalidOperationException">Either no elements or too many.</exception>
        /// <exception cref="System.InvalidCastException">The specified type was incorrect.</exception>
        /// <returns></returns>
        public T GetSingleValue<T>()
        {
            return Elements.Single().GetValue<T>();
        }

        /// <summary>
        /// Gets an array of correctly typed elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="System.InvalidCastException">The specified type was incorrect.</exception>
        /// <returns></returns>
        public T[] GetValues<T>()
        {
            return Elements.Select(item => item.GetValue<T>()).ToArray();
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether the option conforms to the specified option specification.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="optionSpec"></param>
        /// <param name="eventLog"></param>
        /// <returns></returns>
        public bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, ISpecValidatorEventLog eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected interface

        /// <summary>
        /// Observer for <see cref="Elements"/> that keeps the integrity of the
        /// collection's items. At the moment, there is only one invariant: every item's
        /// type has to be equal to <see cref="ValueType"/>.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        /// <exception cref="Ini.Exceptions.InvariantBrokenException">An invariant was broken.</exception>
        protected void OnElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    // check the invariants and throw an exception if required
                    // TODO: reset might behave differently
                    bool elementWithBadTypeFound = false;
                    for(int i = e.NewStartingIndex; i < e.NewItems.Count; i++)
                    {
                        IElement element = (IElement) e.NewItems[i];
                        if(!element.ValueType.Equals(ValueType))
                        {
                            elementWithBadTypeFound = true;
                            Elements.Remove(element);
                        }
                    }
                    if(elementWithBadTypeFound)
                    {
                        throw new InvariantBrokenException(string.Format(
                            "The collection of elements must only consist of elements with value type: {0}.",
                            ValueType.FullName));
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                    // one or more items were moved or removed - that doesn't break any invariant
                    break;

                default:
                    throw new ArgumentException("Unknown enum value: " + e.Action.ToString());
            }
        }

        #endregion

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
