using System;
using Ini.Util.Guid;
using System.Collections.Generic;
using Ini.Configuration.Elements;

namespace Ini.Util.LinkResolving
{
	/// <summary>
	/// Dependency graph node for <see cref="LinkResolver"/>.
	/// </summary>
	public class LinkNode
	{
		/// <summary>
		/// The configuration location where the link originated.
		/// </summary>
		/// <value>The origin.</value>
		public LinkOrigin Origin { get; private set; }

		/// <summary>
		/// The configuration location where the link points to.
		/// </summary>
		/// <value>The target.</value>
		public LinkTarget Target { get; private set; }

		/// <summary>
		/// The link's reference element to replace with the resolved elements.
		/// </summary>
		/// <value>The reference element.</value>
        public IElement RefElement { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Util.LinkResolving.LinkNode"/> class.
		/// </summary>
		/// <param name="refElement">Reference element.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="target">Target.</param>
        public LinkNode(IElement refElement, LinkOrigin origin, LinkTarget target)
		{
			this.RefElement = refElement;
			this.Origin = origin;
			this.Target = target;
		}
	}
}
