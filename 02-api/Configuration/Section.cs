﻿using System;
using System.Collections;
using System.Collections.Generic;
using Ini.Backlogs;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using System.Collections.ObjectModel;
using Ini.Configuration.Elements;

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
		protected ConfigBlockDictionary<string, ConfigBlockBase> Content;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
		public Section(string identifier, string commentary = null) : base(identifier)
        {
			this.TrailingCommentary = commentary;
			this.OptionCount = 0;
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
			Content.Clear();
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
			if(Content.TryGetValue(optionIdentifier, out result))
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
			return Content;
		}

		#endregion

		#region Validation

        /// <summary>
		/// Determines whether the section conforms to the given section specification.
        /// </summary>
        /// <param name="mode"></param>
		/// <param name="sectionSpec"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
		public bool IsValid(SectionSpec sectionSpec, ConfigValidationMode mode, ISpecValidatorBacklog backlog = null)
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
		System.Collections.IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) Content).GetEnumerator();
		}

		#endregion
    }
}
