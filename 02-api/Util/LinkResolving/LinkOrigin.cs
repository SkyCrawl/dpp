using System;
using System.Collections.Generic;

namespace Ini.Util.LinkResolving
{
	/// <summary>
	/// The complete configuration location where the link originated.
	/// </summary>
	public class LinkOrigin : LinkBase
	{
		/// <summary>
		/// The value position where the link originated.
		/// </summary>
		/// <value>The index of the element.</value>
		public uint ElementIndex { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Util.LinkResolving.LinkOrigin"/> class.
		/// </summary>
		/// <param name="section">The section where the link originated.</param>
		/// <param name="option">The option where the link originated.</param>
		/// <param name="elementIndex">The value position where the link originated.</param>
		public LinkOrigin(string section, string option, uint elementIndex) : base(section, option)
		{
			this.ElementIndex = elementIndex;
		}
	}
}
