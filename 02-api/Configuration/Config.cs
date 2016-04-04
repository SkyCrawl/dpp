using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ini.Backlogs;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using Ini.Exceptions;
using Ini.Configuration.Elements;

namespace Ini.Configuration
{
    /// <summary>
    /// Class representing configuration as a whole.
    /// </summary>
	public class Config : IEnumerable<KeyValuePair<string, ConfigBlockBase>>
    {
        #region Properties

		/// <summary>
		/// Readonly origin of this configuration. Can be a system path, a URL or anything else really.
		/// The main purpose of this field is for backlogs to be able to denote what configuration
		/// or specification is being processed.
		/// </summary>
		public string Origin { get; private set; }

        /// <summary>
        /// The schema this configuration should conform to. It can be changed during runtime to
		/// perform checks against user-defined schemas.
        /// </summary>
		public ConfigSpec Spec { get; set; }

		/// <summary>
		/// Gets the count of underlying sections.
		/// </summary>
		/// <value>The count sections.</value>
		public int SectionCount { get; private set; }

		/// <summary>
		/// The configuration's content (instances of <see cref="Section"/> and
		/// <see cref="Commentary"/>) in insertion order.
		/// Direct access is not entirely appropriate because the internal representation may change
		/// in time and consumers of this library should not be made to use a dictionary-specific
		/// solution. Specialized methods are a better choice.
		/// </summary>
		protected ConfigBlockDictionary<string, ConfigBlockBase> Content;

		#endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
		public Config(ConfigSpec schema = null) : this(null, schema)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="Configuration"/> class.
		/// </summary>
		public Config(string origin, ConfigSpec schema = null)
		{
            this.Origin = origin;
			this.Spec = schema;
			this.SectionCount = 0;
			this.Content = new ConfigBlockDictionary<string, ConfigBlockBase>();
		}

        #endregion

		#region Public methods managing content

		/// <summary>
		/// Adds the specified block of content.
		/// </summary>
		/// <exception cref="System.ArgumentException">Content with the same identifier has already been added.</exception>
		/// <param name="content">Content.</param>
		public void Add(ConfigBlockBase content)
		{
			Content.Add(content.Identifier, content);
			if(content is Section)
			{
				SectionCount++;
			}
		}

		/// <summary>
		/// Adds the specified blocks of content.
		/// </summary>
		/// <exception cref="System.ArgumentException">Content with the same identifier has already been added.</exception>
		/// <param name="contents">Content.</param>
		public void AddAll(IEnumerable<ConfigBlockBase> contents)
		{
			foreach(ConfigBlockBase content in contents)
			{
				Add(content);
			}
		}

		/// <summary>
		/// Determines whether the section contains content with the specified identifier.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		public bool Contains(string identifier)
		{
			return Content.ContainsKey(identifier);
		}

		/// <summary>
		/// Gets the content associated to the specified identifier.
		/// </summary>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">Identifier not found.</exception>
		/// <param name="identifier">Identifier.</param>
		public ConfigBlockBase GetContent(string identifier)
		{
			return Content[identifier];
		}

		/// <summary>
		/// Outputs content with the specified identifier and returns true if found.
		/// </summary>
		/// <returns><c>true</c>, if value was found, <c>false</c> otherwise.</returns>
		/// <param name="identifier">Identifier.</param>
		/// <param name="content">Content.</param>
		public bool TryGetContent(string identifier, out ConfigBlockBase content)
		{
			return Content.TryGetValue(identifier, out content);
		}

		/// <summary>
		/// Removes the specified option.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		public bool Remove(string identifier)
		{
			ConfigBlockBase value;
			if(Content.TryGetValue(identifier, out value))
			{
				Content.Remove(identifier);
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
		/// Removes the specified options.
		/// </summary>
		/// <param name="identifiers">Identifier.</param>
		public void RemoveAll(IEnumerable<string> identifiers)
		{
			foreach(string identifier in identifiers)
			{
				Remove(identifier);
			}
		}

		/// <summary>
		/// Clear the whole section.
		/// </summary>
		public void Clear()
		{
			Content.Clear();
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
			if(Content.TryGetValue(identifier, out result))
			{
				return result is Section ? (Section) result : null;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the option with the specified identifier, within the specified section.
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
		/// Gets the collection of elements of the option with the specified identifier, within the specified section.
		/// </summary>
		/// <returns>The collection of elements, or null if not found.</returns>
		/// <param name="sectionIdentifier">Target section identifier.</param>
		/// <param name="optionIdentifier">Target option identifier.</param>
		public ObservableCollection<IElement> GetElements(string sectionIdentifier, string optionIdentifier)
		{
			Option option = GetOption(sectionIdentifier, optionIdentifier);
			return option != null ? option.Elements : null;
		}

		/// <summary>
		/// Gets the specified element of the option with the specified identifier, within the specified section.
		/// </summary>
		/// <returns>The element, or null if not found.</returns>
		/// <param name="sectionIdentifier">Target section identifier.</param>
		/// <param name="optionIdentifier">Target option identifier.</param>
		/// <param name="elementIndex">Target element index.</param>
		public IElement GetElement(string sectionIdentifier, string optionIdentifier, int elementIndex)
		{
			Option option = GetOption(sectionIdentifier, optionIdentifier);
			return option != null ? option.GetElement(elementIndex) : null;
		}

		/// <summary>
		/// Gets the specified element of the option with the specified identifier, within the specified section.
		/// The output type is correctly typed.
		/// </summary>
		/// <returns>The element, or null if not found.</returns>
		/// <param name="sectionIdentifier">Target section identifier.</param>
		/// <param name="optionIdentifier">Target option identifier.</param>
		/// <param name="elementIndex">Target element index.</param>
		/// <exception cref="InvalidCastException">If the specified type was not correct.</exception>
		public T GetElement<T>(string sectionIdentifier, string optionIdentifier, int elementIndex) where T : IElement
		{
			return (T) GetElement(sectionIdentifier, optionIdentifier, elementIndex);
		}

		/// <summary>
		/// Gets the underlying collection, for custom processing or alterations. Use only
		/// if you know what you're doing. <see cref="SectionCount"/> is being managed
		/// manually.
		/// </summary>
		/// <returns>The collection.</returns>
		public ConfigBlockDictionary<string, ConfigBlockBase> GetContent()
		{
			return Content;
		}

		#endregion

		#region Validation

		/// <summary>
		/// Determines whether the configuration conforms to <see cref="Spec"/>.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid the specified mode configBacklog schemaBacklog; otherwise, <c>false</c>.</returns>
		/// <exception cref="UndefinedSpecException">If the specification is undefined.</exception>
		/// <exception cref="InvalidSpecException">If the schema is invalid.</exception>
		/// <param name="mode">Mode.</param>
		/// <param name="configBacklog">Config backlog.</param>
		/// <param name="specBacklog">Specification backlog.</param>
		public bool IsValid(ConfigValidationMode mode, IConfigWriterBacklog configBacklog, ISpecValidatorBacklog specBacklog = null)
		{
			if(Spec == null)
			{
				throw new UndefinedSpecException();
			}
			else if(!Spec.IsValid(specBacklog))
			{
				configBacklog.SpecNotValid();
				throw new InvalidSpecException();
			}
			else
			{
				throw new NotImplementedException();
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
			return ((IEnumerable<KeyValuePair<string, ConfigBlockBase>>) Content).GetEnumerator();
		}

		/// <summary>
		/// Gets the content enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) Content).GetEnumerator();
		}

		#endregion
    }
}
