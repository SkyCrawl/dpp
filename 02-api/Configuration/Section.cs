using System;
using System.Collections;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using System.Collections.ObjectModel;
using Ini.Configuration.Elements;
using System.Collections.Specialized;
using Ini.Exceptions;

namespace Ini.Configuration
{
    /// <summary>
    /// Class representing section as a whole.
    /// </summary>
    public class Section : ConfigBlockBase, IEnumerable<KeyValuePair<string, ConfigBlockBase>>
    {
        #region Properties

        /// <summary>
        /// Trailing commentary for the section.
        /// </summary>
        public string TrailingCommentary { get; set; }

        /// <summary>
        /// Gets the count of underlying options.
        /// </summary>
        /// <value>The count sections.</value>
        public int OptionCount { get; private set; }

        /// <summary>
        /// The section's content (instances of <see cref="Option"/> and
        /// <see cref="Commentary"/>) in insertion order.
        /// </summary>
        protected ConfigBlockDictionary<string, ConfigBlockBase> content;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section(string identifier, string commentary = null) : base(identifier)
        {
            this.TrailingCommentary = commentary;
            this.OptionCount = 0;
            this.content = new ConfigBlockDictionary<string, ConfigBlockBase>();
            this.content.CollectionChanged += new NotifyCollectionChangedEventHandler(OnContentChanged);
        }

        #endregion

        #region Public methods managing content

        /// <summary>
        /// Adds the specified content to the section.
        /// </summary>
        /// <exception cref="System.ArgumentException">Content with the same identifier has already been added.</exception>
        /// <param name="content">Content.</param>
        public void Add(ConfigBlockBase content)
        {
            this.content.Add(content.Identifier, content);
            if(content is Option)
            {
                OptionCount++;
            }
        }

        /// <summary>
        /// Determines whether the section contains content with the specified identifier.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        public bool Contains(string identifier)
        {
            return this.content.ContainsKey(identifier);
        }

        /// <summary>
        /// Gets the content associated to the specified identifier.
        /// </summary>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Identifier not found.</exception>
        /// <param name="identifier">Identifier.</param>
        public ConfigBlockBase GetContent(string identifier)
        {
            return this.content[identifier];
        }

        /// <summary>
        /// Outputs content with the specified identifier and returns true if found.
        /// </summary>
        /// <returns><c>true</c>, if value was found, <c>false</c> otherwise.</returns>
        /// <param name="identifier">Identifier.</param>
        /// <param name="content">Content.</param>
        public bool TryGetContent(string identifier, out ConfigBlockBase content)
        {
            return this.content.TryGetValue(identifier, out content);
        }

        /// <summary>
        /// Removes Adds the specified option.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        public bool Remove(string identifier)
        {
            ConfigBlockBase value;
            if(this.content.TryGetValue(identifier, out value))
            {
                this.content.Remove(identifier);
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
            this.content.Clear();
            OptionCount = 0;
        }

        #endregion

        #region Public methods querying content

        /// <summary>
        /// Gets the option with the specified identifier.
        /// </summary>
        /// <returns>The option, or null if not found.</returns>
        /// <param name="optionIdentifier">Target option identifier.</param>
        public Option GetOption(string optionIdentifier)
        {
            ConfigBlockBase result;
            if(this.content.TryGetValue(optionIdentifier, out result))
            {
                return result is Option ? (Option) result : null;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the collection of elements of the option with the specified identifier.
        /// </summary>
        /// <returns>The collection of elements, or null if not found.</returns>
        /// <param name="optionIdentifier">Target option identifier.</param>
        public ObservableCollection<IElement> GetElements(string optionIdentifier)
        {
            Option option = GetOption(optionIdentifier);
            return option != null ? option.Elements : null;
        }

        /// <summary>
        /// Gets the specified element of the option with the specified identifier.
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
        /// Gets the specified element of the option with the specified identifier.
        /// The output type is correctly typed.
        /// </summary>
        /// <returns>The element, or null if not found.</returns>
        /// <param name="optionIdentifier">Target option identifier.</param>
        /// <param name="elementIndex">Target element index.</param>
        /// <exception cref="InvalidCastException">If the specified type was not correct.</exception>
        public T GetElement<T>(string optionIdentifier, int elementIndex) where T : IElement
        {
            return (T) GetElement(optionIdentifier, elementIndex);
        }

        /// <summary>
        /// Gets the underlying collection, for custom processing or alterations. Use only
        /// if you know what you're doing. <see cref="OptionCount"/> is being managed
        /// manually.
        /// </summary>
        /// <returns>The collection.</returns>
        public ConfigBlockDictionary<string, ConfigBlockBase> GetContent()
        {
            return this.content;
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether the section conforms to the given section specification.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="sectionSpec"></param>
        /// <param name="eventLog"></param>
        /// <returns></returns>
        public bool IsValid(SectionSpec sectionSpec, ConfigValidationMode mode, ISpecValidatorEventLogger eventLog = null)
        {
            throw new NotImplementedException();
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
                    foreach(ConfigBlockBase item in e.NewItems)
                    {
                        if(item is Section)
                        {
                            throw new InvariantBrokenException(string.Format(
                                "'{0}' can not contain instances of '{1}'.",
                                this.GetType().ToString(),
                                item.GetType().ToString()));
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

        #region IEnumerable implementation

        /// <summary>
        /// Gets the content enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<KeyValuePair<string, ConfigBlockBase>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, ConfigBlockBase>>) content).GetEnumerator();
        }

        /// <summary>
        /// Gets the content enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) content).GetEnumerator();
        }

        #endregion
    }
}
