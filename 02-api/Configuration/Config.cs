using System;
using System.Collections;
using System.Collections.Generic;
using Ini.Backlogs;
using Ini.Schema;
using Ini.Util;

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
		public ConfigSpec Schema { get; set; }

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
		private ConfigBlockDictionary<string, ConfigBlockBase> Content;

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
			this.Schema = schema;
			this.SectionCount = 0;
			this.Content = new ConfigBlockDictionary<string, ConfigBlockBase>();
		}

        #endregion

		#region Public methods managing content

		/// <summary>
		/// Adds the specified content.
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
		/// Removes Adds the specified option.
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
		/// Clear the whole section.
		/// </summary>
		public void Clear()
		{
			Content.Clear();
			SectionCount = 0;
		}

		/// <summary>
		/// Gets the underlying collection, for custom processing or alterations. Use only
		/// if you know what you're doing. <see cref="SectionCount"/> is being managed
		/// manually.
		/// </summary>
		/// <returns>The collection.</returns>
		public ConfigBlockDictionary<string, ConfigBlockBase> GetCollection()
		{
			return Content;
		}

		#endregion

		#region Validation

		/// <summary>
		/// Determines whether the configuration conforms to <see cref="Schema"/>.
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="backlog"></param>
		/// <returns></returns>
		public bool IsValid(ValidationMode mode, IValidationBacklog backlog = null)
		{
			throw new NotImplementedException();
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
