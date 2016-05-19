using Ini.Configuration.Base;

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
        /// The link's configuration element. When the link is resolved,
        /// <see cref="LinkResolver"/> will update the element accordingly.
        /// </summary>
        /// <value>The reference element.</value>
        public ILink LinkElement { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Util.LinkResolving.LinkNode"/> class.
        /// </summary>
        /// <param name="linkElement">The link's configuration element.</param>
        /// <param name="origin">The link's origin.</param>
        public LinkNode(ILink linkElement, LinkOrigin origin)
        {
            this.Origin = origin;
            this.Target = linkElement.Target;
            this.LinkElement = linkElement;
        }
    }
}
