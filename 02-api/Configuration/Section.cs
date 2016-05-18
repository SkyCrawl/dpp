using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Ini.Exceptions;
using Ini.Configuration.Base;
using Ini.Configuration.Values;
using System.IO;

namespace Ini.Configuration
{
    /// <summary>
    /// Class representing section as a whole.
    /// </summary>
    public class Section : ConfigBlockBase, IEnumerable<KeyValuePair<string, ConfigBlockBase>>
    {
        #region Properties

        /// <summary>
        /// The section's trailing commentary.
        /// </summary>
        /// <value>The section's trailing commentary.</value>
        public string TrailingCommentary { get; set; }

        /// <summary>
        /// The position of the trailing commentary.
        /// </summary>
        public int TrailingCommentaryPosition { get; set; }

        /// <summary>
        /// Gets the count of underlying options.
        /// </summary>
        /// <value>The count sections.</value>
        public int OptionCount { get; private set; }

        /// <summary>
        /// The section's content (instances of <see cref="Option"/> and
        /// <see cref="Commentary"/>) in insertion order. Use only if you know what you're doing.
        /// <see cref="OptionCount"/> is being managed manually.
        /// </summary>
        public ObservableInsertionDictionary<string, ConfigBlockBase> Items { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section(string identifier, string commentary = null, int commentaryPosition = 0)
            : base(identifier)
        {
            TrailingCommentary = commentary;
            TrailingCommentaryPosition = commentaryPosition;

            OptionCount = 0;
            Items = new ObservableInsertionDictionary<string, ConfigBlockBase>(new NotifyCollectionChangedEventHandler(OnContentChanged));
        }

        #endregion

        #region Public methods managing content

        /// <summary>
        /// Adds the specified item to the section.
        /// </summary>
        /// <exception cref="ArgumentException">Item with the same identifier has already been added.</exception>
        /// <param name="item">The item to add.</param>
        public void Add(ConfigBlockBase item)
        {
            this.Items.Add(item.Identifier, item);
            if(item is Option)
            {
                OptionCount++;
            }
        }

        /// <summary>
        /// Determines whether the section contains an item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public bool Contains(string identifier)
        {
            return this.Items.ContainsKey(identifier);
        }

        /// <summary>
        /// Gets the item associated to the specified identifier.
        /// </summary>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Identifier not found.</exception>
        /// <param name="identifier">The identifier.</param>
        public ConfigBlockBase GetContent(string identifier)
        {
            return this.Items[identifier];
        }

        /// <summary>
        /// Outputs item with the specified identifier and returns true if found.
        /// </summary>
        /// <returns><c>true</c>, if value was found, <c>false</c> otherwise.</returns>
        /// <param name="identifier">The identifier.</param>
        /// <param name="item">The item.</param>
        public bool TryGetContent(string identifier, out ConfigBlockBase item)
        {
            return this.Items.TryGetValue(identifier, out item);
        }

        /// <summary>
        /// Removes item with the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public bool Remove(string identifier)
        {
            ConfigBlockBase value;
            if(this.Items.TryGetValue(identifier, out value))
            {
                this.Items.Remove(identifier);
                if(value is Option)
                {
                    OptionCount--;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Clear the whole section.
        /// </summary>
        public void Clear()
        {
            this.Items.Clear();
            OptionCount = 0;
        }

        #endregion

        #region Public methods querying content

        /// <summary>
        /// Gets option with the specified identifier.
        /// </summary>
        /// <returns>The option, or null if not found.</returns>
        /// <param name="optionIdentifier">Target option identifier.</param>
        public Option GetOption(string optionIdentifier)
        {
            ConfigBlockBase result;
            if(this.Items.TryGetValue(optionIdentifier, out result))
            {
                return result is Option ? result as Option : null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all instances of <see cref="Option"/> within the section.
        /// </summary>
        /// <returns>All sections.</returns>
        public IEnumerable<Option> GetAllOptions()
        {
            foreach(ConfigBlockBase item in Items.Values)
            {
                if(item is Option)
                {
                    yield return item as Option;
                }
            }
            yield break;
        }

        /// <summary>
        /// Gets the specified option's collection of elements.
        /// </summary>
        /// <returns>The collection of elements, or null if not found.</returns>
        /// <param name="optionIdentifier">Target option identifier.</param>
        public ObservableList<IElement> GetElements(string optionIdentifier)
        {
            Option option = GetOption(optionIdentifier);
            return option != null ? option.Elements : null;
        }

        /// <summary>
        /// Gets the specified option's element, with the specified index.
        /// </summary>
        /// <returns>The element, or null if not found.</returns>
        /// <param name="optionIdentifier">Target option identifier.</param>
        /// <param name="elementIndex">Target element index.</param>
        public IElement GetElement(string optionIdentifier, int elementIndex)
        {
            Option option = GetOption(optionIdentifier);
            return option != null ? option.GetElement(elementIndex) : null;
        }

        /// <summary>
        /// Gets the specified option's element (correctly typed), with the specified index.
        /// </summary>
        /// <returns>The element, or null if not found.</returns>
        /// <param name="optionIdentifier">Target option identifier.</param>
        /// <param name="elementIndex">Target element index.</param>
        /// <exception cref="InvalidCastException">If the specified type was not correct.</exception>
        public T GetElement<T>(string optionIdentifier, int elementIndex) where T : IValue
        {
            return (T) GetElement(optionIdentifier, elementIndex);
        }

        /// <summary>
        /// Gets the specified option's and element's value, correctly typed.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="optionIdentifier">Target option's identifier.</param>
        /// <param name="elementIndex">Target element index.</param>
        /// <exception cref="InvalidCastException">The specified type was incorrect.</exception>
        /// <exception cref="IndexOutOfRangeException">If the index is out of range, or the option is not found.</exception>
        /// <exception cref="InvalidOperationException">Target element is a link and it contains zero or multiple
        /// values, or it was an instance of <see cref="ValueStub"/>, or it was an unknown subtype of 
        /// <see cref="IElement"/>.</exception>
        public OutputType GetValue<OutputType>(string optionIdentifier, int elementIndex)
        {
            Option option = GetOption(optionIdentifier);
            if(option == null)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return option.GetValue<OutputType>(elementIndex);
            }
        }

        /// <summary>
        /// Converts the specified option's elements into a collection of value (<see cref="IValue"/>) objects.
        /// </summary>
        /// <exception cref="ArgumentException">An element's type is not handled in the underlying method.</exception>
        /// <returns>The collection.</returns>
        public IList<IValue> GetObjectValues(string optionIdentifier)
        {
            Option option = GetOption(optionIdentifier);
            return option != null ? option.GetObjectValues() : null;
        }

        /// <summary>
        /// Converts the specified option's elements into an array of elementary values.
        /// </summary>
        /// <returns>The array.</returns>
        /// <typeparam name="OutputType">The correct type.</typeparam>
        /// <exception cref="ArgumentException">An element's type is not handled in the underlying method.</exception>
        /// <exception cref="System.InvalidCastException">The specified type was incorrect.</exception>
        public OutputType[] GetValues<OutputType>(string optionIdentifier)
        {
            return GetObjectValues(optionIdentifier).GetValues<OutputType>();
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether the section conforms to the given section specification.
        /// </summary>
        /// <returns><c>true</c> if this instance validates against the given mode and specification; otherwise, <c>false</c>.</returns>
        /// <param name="config">The parent configuration object.</param>
        /// <param name="sectionSpec">The section specification.</param>
        /// <param name="mode">Validation mode to use.</param>
        /// <param name="logger">Configuration validation event logger.</param>
        public bool IsValid(Config config, SectionSpec sectionSpec, ConfigValidationMode mode, IConfigValidatorEventLogger logger)
        {
            // prepare the result validation state
            bool result = true;

            // validate the inner structure against the specification
            foreach(Option option in Items.Values.Where(item => item is Option))
            {
                OptionSpec optionSpec = sectionSpec.GetOption(option.Identifier);
                if(optionSpec == null)
                {
                    // okay, that's something we should know about
                    logger.NoOptionSpecification(sectionSpec.Identifier, option.Identifier);

                    // and error status depends on the validation mode
                    result = mode == ConfigValidationMode.Relaxed;
                }
                else if(!option.IsValid(config, Identifier, optionSpec, logger))
                {
                    result = false;
                }
            }

            // validate that the configuration contains all mandatory options
            if(mode == ConfigValidationMode.Strict)
            {
                foreach(OptionSpec mandatoryOption in sectionSpec.Options.Where(item => item.IsMandatory))
                {
                    if(!Contains(mandatoryOption.Identifier))
                    {
                        logger.MissingMandatoryOption(sectionSpec.Identifier, mandatoryOption.Identifier);
                        result = false;
                    }
                }
            }

            // and return
            return result;
        }

        #endregion

        #region Keeping internal state

        /// <summary>
        /// The delegate for <see cref="INotifyCollectionChanged"/>.
        /// </summary>
        /// <param name="sender">The observed collection.</param>
        /// <param name="e">Changes that occurred.</param>
        protected void OnContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                foreach(KeyValuePair<string, ConfigBlockBase> entry in e.NewItems)
                    {
                        if(entry.Key != entry.Value.Identifier)
                        {
                            throw new InvariantBrokenException(string.Format(
                                "Configuration block must be mapped to its identifier. Got: '{0}'. Expected: '{1}'.",
                                entry.Key,
                                entry.Value.Identifier));
                        }
                        else if(entry.Value is Section)
                        {
                            throw new InvariantBrokenException(string.Format(
                                "'{0}' can not contain instances of '{1}'.",
                                this.GetType().ToString(),
                                entry.Value.GetType().ToString()));
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    // one or more items were moved or removed - that doesn't break any invariant
                    break;

                default:
                    throw new ArgumentException("Unknown enum value: " + e.Action.ToString());
            }
        }

        #endregion

        #region ConfigBlockBase Members

        /// <summary>
        /// Serializes this instance into the specified text writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="options">Serialization options.</param>
        /// <param name="sectionSpecification">Section specification of the current configuration block.</param>
        /// <param name="config">The parent configuration.</param>
        internal override void SerializeSelf(TextWriter writer, ConfigWriterOptions options, SectionSpec sectionSpecification, Config config)
        {
            // first serialize the header
            var header = IniSyntax.SerializeSectionHeader(Identifier);
            var commentarySpacesCount = TrailingCommentaryPosition - header.Length;

            writer.Write(header);
            writer.WriteLine(IniSyntax.SerializeCommentary(TrailingCommentary, commentarySpacesCount > 1 ? commentarySpacesCount : 1));

            // and then inner options
            foreach(ConfigBlockBase item in Items.ReorderBlocks(sectionSpecification == null ? null : sectionSpecification.Options, options.SectionSortOrder))
            {
                item.SerializeSelf(writer, options, sectionSpecification, config);
            }
        }

        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Gets the content enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<KeyValuePair<string, ConfigBlockBase>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, ConfigBlockBase>>) Items).GetEnumerator();
        }

        /// <summary>
        /// Gets the content enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Items).GetEnumerator();
        }

        #endregion
    }
}
