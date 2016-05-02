using System;
using System.Collections.Generic;
using System.Linq;

namespace Ini.Util.LinkResolving
{
	/// <summary>
	/// As denoted by inheritance, each bucket corresponds to a particular option within
    /// a particular section. It indexes links (<see cref="LinkNode"/>) defined within
    /// that option. Before each link can be resolved, however, its target option (bucket)
    /// has to be resolved first. Option, mind you, not individual option value.
	/// </summary>
	public class LinkBucket : LinkBase
	{
		/// <summary>
		/// All links (<see cref="LinkNode"/>) indexed by this bucket.
		/// </summary>
		/// <value>The links.</value>
        public List<LinkNode> Links { get; private set; }

		/// <summary>
		/// All buckets (<see cref="LinkBucket"/>) this bucket depends on.
        /// Note: Make sure NOT to make a set out of this collection.
		/// </summary>
		/// <value>The dependencies.</value>
		public List<LinkBucket> DependsOnBuckets { get; private set; }

		/// <summary>
		/// All buckets (<see cref="LinkBucket"/>) dependent on this bucket.
        /// Note: Make sure NOT to make a set out of this collection.
		/// </summary>
		/// <value>The dependants.</value>
		public List<LinkBucket> Dependants { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Util.LinkResolving.LinkBucket"/> class.
		/// </summary>
		/// <param name="section">The section where the link originated.</param>
		/// <param name="option">The option where the link originated.</param>
		public LinkBucket(string section, string option) : base(section, option)
		{
			this.Links = new List<LinkNode>();
			this.DependsOnBuckets = new List<LinkBucket>();
			this.Dependants = new List<LinkBucket>();
		}

		/// <summary>
		/// Determines whether this bucket is ready to be resolved. In other words,
        /// either the bucket is independent or all the buckets it depends on are
        /// resolved.
		/// </summary>
		/// <returns><c>true</c> if this bucket is ready to be resolved; otherwise, <c>false</c>.</returns>
		public bool IsReadyToBeResolved(LinkResolver resolver)
		{
			return (DependsOnBuckets.Count == 0) || DependsOnBuckets.All(bucket => resolver.IsBucketResolved(bucket));
		}
	}
}
