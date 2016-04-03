using System;
using System.Collections.Generic;
using Ini.Util.Guid;

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

		/// <summary>
		/// Static identifier generator for instances of this class.
		/// </summary>
		public static IGuid<string> identifierGenerator = new SystemGuidGenerator();

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Configuration.Commentary"/> class.
		/// </summary>
		public Commentary() : base(identifierGenerator.Next())
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
