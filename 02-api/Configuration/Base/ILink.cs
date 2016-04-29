using System;
using Ini.Util.LinkResolving;

namespace Ini.Configuration.Base
{
    /// <summary>
    /// I link.
    /// </summary>
    public interface ILink : IElement
    {
        /// <summary>
        /// This link's target.
        /// </summary>
        /// <value>The target.</value>
        LinkTarget Target { get; }

        /// <summary>
        /// An indicator whether this link has been resolved.
        /// </summary>
        /// <value><c>true</c> if this link has been resolved; otherwise, <c>false</c>.</value>
        bool IsResolved { get; }

        /// <summary>
        /// Looks into <see cref="Target"/> and if it's resolved, updates the inner data accordingly.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        void Resolve(Config config);
    }
}
