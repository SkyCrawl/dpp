using System;
using System.Collections.Generic;

namespace Ini.Configuration
{
	/// <summary>
	/// Represents a block of empty and commentary lines.
	/// </summary>
	public class Commentary : ConfigBlockBase, IEnumerable<string>
	{
		#region Properties
		/// <summary>
		/// A block of empty and commentary lines.
		/// </summary>
		public List<string> Lines { get; private set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Configuration.Commentary"/> class.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		public Commentary(string identifier) : base(identifier)
		{
			this.Lines = new List<string>();
		}

		#endregion

		#region IEnumerable implementation

		/// <summary>
		/// Gets the content enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<string> GetEnumerator()
		{
			return Lines.GetEnumerator();
		}

		/// <summary>
		/// Gets the content enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Lines.GetEnumerator();
		}

		#endregion
	}
}
