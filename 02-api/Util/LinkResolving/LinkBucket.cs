using System;
using System.Collections.Generic;
using System.Linq;

namespace Ini.Util.LinkResolving
{
	/// <summary>
	/// Buckets used to index links (<see cref="LinkNode"/>) in <see cref="LinkResolver"/>.
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
		/// </summary>
		/// <value>The dependencies.</value>
		public List<LinkBucket> DependsOnBuckets { get; private set; }

		/// <summary>
		/// All buckets (<see cref="LinkBucket"/>) dependent on this bucket.
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
		/// Determines whether this bucket is independent.
		/// </summary>
		/// <returns><c>true</c> if this bucket is independent; otherwise, <c>false</c>.</returns>
		public bool IsReadyToBeResolved(LinkResolver resolver)
		{
			return (DependsOnBuckets.Count == 0) || DependsOnBuckets.All(bucket => resolver.IsBucketResolved(bucket));
		}
	}
}
