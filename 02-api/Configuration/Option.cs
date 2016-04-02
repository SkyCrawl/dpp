using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ini.Schema;
using Ini.Configuration.Elements;
using Ini.Backlogs;
using Ini.Util;

namespace Ini.Configuration
{
    /// <summary>
	/// Class representing option as a whole.
    /// </summary>
	public class Option : ConfigBlockBase, IEnumerable<IElement>
    {
        #region Properties

		/// <summary>
		/// Readonly type of all the option's elements. Should you wish to change
		/// the type, it's better to create a whole new <see cref="Option"/>, perhaps
		/// using the same identifier.
		/// </summary>
		public Type ElementType { get; private set; }

		/// <summary>
		/// Trailing commentary for the option.
		/// </summary>
		public string TrailingCommentary { get; set; }

        /// <summary>
		/// This option's value, consisting of subclasses of <see cref="Element{T}"/>.
        /// </summary>
		private List<IElement> Elements;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class.
        /// </summary>
		public Option(string identifier, Type elementType, string commentary = null) : base(identifier)
        {
			this.ElementType = elementType;
			this.TrailingCommentary = commentary;
			this.Elements = new List<IElement>();
        }

        #endregion

		#region Public methods managing elements




		#endregion

		#region Other public methods



		/// <summary>
		/// Retuns the typed option value for options with single element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetValue<T>()
		{
			return Elements.Single().GetValue<T>();
		}

		/// <summary>
		/// Returns the array typed option values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
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
		/// <param name="definition"></param>
		/// <param name="backlog"></param>
		/// <returns></returns>
		public bool IsValid(ValidationMode mode, OptionSpec definition, IValidationBacklog backlog = null)
		{
			throw new NotImplementedException();
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
