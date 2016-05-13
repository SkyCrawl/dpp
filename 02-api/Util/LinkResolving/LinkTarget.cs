using System;

namespace Ini.Util.LinkResolving
{
    /// <summary>
    /// The complete configuration location where the link points to. Should it be
    /// ever needed, this is the place to define dependency on specific values of
    /// an option. But then, <see cref="LinkBucket.IsReadyToBeResolved"/> needs to
    /// be updated to respect such lightweight dependency and resolving has to be
    /// done with individual links, not buckets.
    /// </summary>
    public class LinkTarget : LinkBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Util.LinkResolving.LinkTarget"/> class.
        /// </summary>
        /// <param name="section">The section where the link points to.</param>
        /// <param name="option">The option where the link points to.</param>
        public LinkTarget(string section, string option) : base(section, option)
        {
        }
    }
}
