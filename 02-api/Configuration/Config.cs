using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Exceptions;
using Ini.Configuration.Base;
using Ini.Configuration.Values;
using System.IO;

namespace Ini.Configuration
{
    /// <summary>
    /// Class representing configuration as a whole.
    /// </summary>
    public class Config : IEnumerable<KeyValuePair<string, ConfigBlockBase>>
    {
        #region Properties

        /// <summary>
        /// The schema this configuration should conform to. It can be changed during runtime to
        /// perform checks against user-defined schemas.
        /// </summary>
        public ConfigSpec Spec { get; private set; }

        /// <summary>
        /// Readonly origin of this configuration. Can be a system path, a URL or anything else really.
        /// The main purpose of this field is for event logs to be able to denote what configuration
        /// or specification is being processed.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Gets the count of underlying sections.
        /// </summary>
        /// <value>The count sections.</value>
        public int SectionCount { get; private set; }

        /// <summary>
        /// The configuration's content (instances of <see cref="Section"/> and
        /// <see cref="Commentary"/>) in insertion order. Use only you know what you're doing.
        /// <see cref="SectionCount"/> is being managed manually.
        /// </summary>
        public ObservableInsertionDictionary<string, ConfigBlockBase> Items { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Config(ConfigSpec schema = null)
        {
            this.Spec = schema;
            this.Origin = null;
            this.SectionCount = 0;
            this.Items = new ObservableInsertionDictionary<string, ConfigBlockBase>(new NotifyCollectionChangedEventHandler(OnContentChanged));
        }

        #endregion

        #region Public methods managing content

        /// <summary>
        /// Adds the specified item to the configuration.
        /// </summary>
        /// <exception cref="System.ArgumentException">Content with the same identifier has already been added.</exception>
        /// <param name="item">The item.</param>
        public void Add(ConfigBlockBase item)
        {
            this.Items.Add(item.Identifier, item);
            if(item is Section)
            {
                SectionCount++;
            }
        }

        /// <summary>
        /// Adds the specified items to the configuration.
        /// </summary>
        /// <exception cref="System.ArgumentException">Content with the same identifier has already been added.</exception>
        /// <param name="items">The item.</param>
        public void AddAll(IEnumerable<ConfigBlockBase> items)
        {
            foreach(ConfigBlockBase content in items)
            {
                Add(content);
            }
        }

        /// <summary>
        /// Determines whether the configuration contains the specified identifier. This
        /// method only inspects direct children.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public bool Contains(string identifier)
        {
            return this.Items.ContainsKey(identifier);
        }

        /// <summary>
        /// Gets the item associated with the specified identifier. This method only inspects
        /// direct children.
        /// </summary>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Identifier not found.</exception>
        /// <param name="identifier">The identifier.</param>
        public ConfigBlockBase GetContent(string identifier)
        {
            return this.Items[identifier];
        }

        /// <summary>
        /// Outputs the item with the specified identifier. This method only inspects direct children.
        /// </summary>
        /// <returns><c>true</c>, if the item was found, <c>false</c> otherwise.</returns>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">Content.</param>
        public bool TryGetContent(string identifier, out ConfigBlockBase content)
        {
            return this.Items.TryGetValue(identifier, out content);
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
                if(value is Section)
                {
                    SectionCount--;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes items with the specified identifiers.
        /// </summary>
        /// <param name="identifiers">The identifier.</param>
        public void RemoveAll(IEnumerable<string> identifiers)
        {
            foreach(string identifier in identifiers)
            {
                Remove(identifier);
            }
        }

        /// <summary>
        /// Clear the whole configuration.
        /// </summary>
        public void Clear()
        {
            this.Items.Clear();
            SectionCount = 0;
        }

        #endregion

        #region Public methods querying content

        /// <summary>
        /// Gets the section with the specified identifier.
        /// </summary>
        /// <returns>The section, or null if not found.</returns>
        /// <param name="identifier">Target section identifier.</param>
        public Section GetSection(string identifier)
        {
            ConfigBlockBase result;
            if(this.Items.TryGetValue(identifier, out result))
            {
                return result is Section ? (Section) result : null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all instances of <see cref="Section"/> within the configuration.
        /// </summary>
        /// <returns>All sections.</returns>
        public IEnumerable<Section> GetAllSections()
        {
            foreach(ConfigBlockBase item in Items.Values)
            {
                if(item is Section)
                {
                    yield return item as Section;
                }
            }
            yield break;
        }

        /// <summary>
        /// Gets the specified option.
        /// </summary>
        /// <returns>The option, or null if not found.</returns>
        /// <param name="sectionIdentifier">Target section identifier.</param>
        /// <param name="optionIdentifier">Target option identifier.</param>
        public Option GetOption(string sectionIdentifier, string optionIdentifier)
        {
            Section section = GetSection(sectionIdentifier);
            return section != null ? section.GetOption(optionIdentifier) : null;
        }

        /// <summary>
        /// Gets the specified option's collection of elements.
        /// </summary>
        /// <returns>The collection of elements, or null if not found.</returns>
        /// <param name="sectionIdentifier">Target section identifier.</param>
        /// <param name="optionIdentifier">Target option identifier.</param>
        public ObservableList<IElement> GetElements(string sectionIdentifier, string optionIdentifier)
        {
            Section section = GetSection(sectionIdentifier);
            return section != null ? section.GetElements(optionIdentifier) : null;
        }

        /// <summary>
        /// Gets the specified option's element, with the specified index.
        /// </summary>
        /// <returns>The element, or null if not found.</returns>
        /// <param name="sectionIdentifier">Target section identifier.</param>
        /// <param name="optionIdentifier">Target option identifier.</param>
        /// <param name="elementIndex">Target element index.</param>
        public IElement GetElement(string sectionIdentifier, string optionIdentifier, int elementIndex)
        {
            Section section = GetSection(sectionIdentifier);
            return section != null ? section.GetElement(optionIdentifier, elementIndex) : null;
        }

        /// <summary>
        /// Gets the specified option's element (correctly typed), with the specified index.
        /// </summary>
        /// <returns>The element, or null if not found.</returns>
        /// <param name="sectionIdentifier">Target section identifier.</param>
        /// <param name="optionIdentifier">Target option identifier.</param>
        /// <param name="elementIndex">Target element index.</param>
        /// <exception cref="InvalidCastException">If the specified type was not correct.</exception>
        public T GetElement<T>(string sectionIdentifier, string optionIdentifier, int elementIndex) where T : IValue
        {
            Section section = GetSection(sectionIdentifier);
            return section != null ? section.GetElement<T>(optionIdentifier, elementIndex) : default(T);
        }

        /// <summary>
        /// Gets the specified option's and element's value, correctly typed.
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="sectionIdentifier">Target section identifier.</param>
        /// <param name="optionIdentifier">Target option's identifier.</param>
        /// <param name="elementIndex">Target element index.</param>
        /// <exception cref="InvalidCastException">The specified type was incorrect.</exception>
        /// <exception cref="IndexOutOfRangeException">If the index is out of range, or the section/option is not found.</exception>
        /// <exception cref="InvalidOperationException">Target element is a link and it contains zero or multiple
        /// values, or it was an instance of <see cref="ValueStub"/>, or it was an unknown subtype of 
        /// <see cref="IElement"/>.</exception>
        public OutputType GetValue<OutputType>(string sectionIdentifier, string optionIdentifier, int elementIndex)
        {
            Section section = GetSection(sectionIdentifier);
            if(section == null)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                return section.GetValue<OutputType>(optionIdentifier, elementIndex);
            }
        }

        /// <summary>
        /// Converts the specified option's elements into a collection of value (<see cref="IValue"/>) objects.
        /// </summary>
        /// <param name="sectionIdentifier">Target section identifier.</param>
        /// <param name="optionIdentifier">Target option identifier.</param>
        /// <exception cref="ArgumentException">An element's type is not handled in the underlying method.</exception>
        /// <returns>The collection.</returns>
        public IList<IValue> GetObjectValues(string sectionIdentifier, string optionIdentifier)
        {
            Section section = GetSection(sectionIdentifier);
            return section != null ? section.GetObjectValues(optionIdentifier) : null;
        }

        /// <summary>
        /// Converts the specified option's elements into an array of elementary values.
        /// </summary>
        /// <returns>The array.</returns>
        /// <typeparam name="OutputType">The correct type.</typeparam>
        /// <exception cref="ArgumentException">An element's type is not handled in the underlying method.</exception>
        /// <exception cref="System.InvalidCastException">The specified type was incorrect.</exception>
        public OutputType[] GetValues<OutputType>(string sectionIdentifier, string optionIdentifier)
        {
            return GetObjectValues(sectionIdentifier, optionIdentifier).GetValues<OutputType>();
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether the configuration conforms to <see cref="Spec"/>.
        /// </summary>
        /// <returns><c>true</c> if this instance validates against the given mode and specification; otherwise, <c>false</c>.</returns>
        /// <param name="mode">Validation mode to use.</param>
        /// <param name="logger">Configuration validation event logger.</param>
        public bool IsValid(ConfigValidationMode mode, IConfigValidatorEventLogger logger)
        {
            // verify the associated specification
            ThrowIfSpecUndefinedOrInvalid(
                logger.SpecValidatiorLogger,
                () => logger.NoSpecification(),
                () => logger.InvalidSpecification()
            );

            // prepare the result validation state
            bool result = true;

            // validate the inner structure against the specification
            foreach(Section section in Items.Values.Where(item => item is Section))
            {
                SectionSpec sectionSpecification = Spec.GetSection(section.Identifier);
                if(sectionSpecification == null)
                {
                    // okay, that's something we should know about
                    logger.NoSectionSpecification(section.Identifier);

                    // and error status depends on the validation mode
                    result = mode == ConfigValidationMode.Relaxed;
                }
                else if(!section.IsValid(this, sectionSpecification, mode, logger))
                {
                    result = false;
                }
            }

            // validate that the configuration contains all mandatory options
            if(mode == ConfigValidationMode.Strict)
            {
                foreach(SectionSpec mandatorySection in Spec.Sections.Where(item => item.IsMandatory))
                {
                    if(!Contains(mandatorySection.Identifier))
                    {
                        logger.MissingMandatorySection(mandatorySection.Identifier);
                        result = false;
                    }
                }
            }

            // and return
            return result;
        }

        /// <summary>
        /// Determines if <see cref="Spec"/> is defined and valid. If not, calls the appropriate action
        /// and throws the appropriate exception.
        /// </summary>
        /// <returns><c>true</c> if specification is defined and valid; otherwise, <c>false</c>.</returns>
        /// <param name="specValidationEventLogger">Configuration validation logger.</param>
        /// <param name="noSpecAction">The action to execute if no specification is defined.</param>
        /// <param name="invalidSpecAction">The action to execute if specification is defined but invalid.</param>
        /// <exception cref="UndefinedSpecException">If no specification is defined.</exception>
        /// <exception cref="InvalidSpecException">If specification is defined but invalid.</exception>
        public void ThrowIfSpecUndefinedOrInvalid(ISpecValidatorEventLogger specValidationEventLogger, Action noSpecAction, Action invalidSpecAction)
        {
            if(Spec == null)
            {
                noSpecAction.Invoke();
                throw new UndefinedSpecException();
            }
            else if(!Spec.IsValid(specValidationEventLogger))
            {
                invalidSpecAction.Invoke();
                throw new InvalidSpecException(); // raise an exception or face undefined behaviour
            }
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
                        else if(entry.Value is Option)
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

        #region Writing configuration

        /// <summary>
        /// Serializes this instance into the specified text writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="options">Serialization options.</param>
        /// <param name="logger">Configuration writer event logger.</param>
        internal void SerializeSelf(TextWriter writer, ConfigWriterOptions options, IConfigWriterEventLogger logger)
        {
            if(options.IsSpecRequiredForSerialization() && (Spec == null))
            {
                logger.NoSpecification();
                throw new UndefinedSpecException();
            }
            else
            {
                foreach(ConfigBlockBase item in Items.ReorderBlocks(Spec == null ? null : Spec.Sections, options.SectionSortOrder))
                {
                    item.SerializeSelf(writer, options, Spec == null ? null : Spec.GetSection(item.Identifier), this);
                }
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
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) Items).GetEnumerator();
        }

        #endregion
    }
}
