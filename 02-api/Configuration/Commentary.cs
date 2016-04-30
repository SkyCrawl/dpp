using System;
using System.Collections;
using System.Collections.Generic;
using Ini.Util.Guid;
using Ini.Configuration.Base;

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
		/// Initializes a new instance of the <see cref="Commentary"/> class.
		/// </summary>
        public Commentary(IEnumerable<string> lines) : base(identifierGenerator.Next())
		{
			this.Lines = new List<string>(lines);
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
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Lines.GetEnumerator();
		}

		#endregion
	}
}
