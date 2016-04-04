using System;
using System.Collections.Generic;
using System.Linq;
using Ini.Configuration;
using Ini.Exceptions;
using Ini.Configuration.Elements;
using Ini.EventLogs;

namespace Ini.Util.LinkResolving
{
	/// <summary>
	/// Dependency "graph" implementation for configuration links.
	/// The general contract is to add links as long as needed, and then
	/// process them.
	/// </summary>
	public class LinkResolver
	{
		/// <summary>
		/// All resolved buckets, indexed by their key.
		/// </summary>
		protected Dictionary<int, LinkBucket> resolvedBuckets;

		/// <summary>
		/// All unresolved buckets, indexed by their key.
		/// </summary>
		protected Dictionary<int, LinkBucket> unresolvedBuckets;

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Util.LinkResolving.LinkResolver"/> class.
		/// </summary>
		public LinkResolver()
		{
			this.resolvedBuckets = new Dictionary<int, LinkBucket>();
			this.unresolvedBuckets = new Dictionary<int, LinkBucket>();
		}

		/// <summary>
		/// Adds a link with the specified origin and target into the graph
		/// and updates dependencies.
		/// </summary>
		/// <param name="refElement">Reference element.</param>
		/// <param name="origin">Origin.</param>
		/// <param name="target">Target.</param>
        public void AddLink(IElement refElement, LinkOrigin origin, LinkTarget target)
		{
			// first some precondition checks
			if(!origin.IsKeySourceValid())
			{
				throw new ArgumentException("Could not determine the link's origin key because the source data (section or option) is invalid.");
			}
			if(!target.IsKeySourceValid())
			{
				throw new ArgumentException("Could not determine the link's target key because the source data (section or option) is invalid.");
			}

			// index the link's origin bucket
			int originKey = origin.ToKey();
			if(!unresolvedBuckets.ContainsKey(originKey))
			{
				unresolvedBuckets[originKey] = new LinkBucket(origin.Section, origin.Option);
			}

			// index the link's target bucket
			int targetKey = target.ToKey();
			if(!unresolvedBuckets.ContainsKey(targetKey))
			{
				unresolvedBuckets[targetKey] = new LinkBucket(target.Section, target.Option);
			}

			// update the dependency graph
			LinkBucket originBucket = unresolvedBuckets[originKey];
			LinkBucket targetBucket = unresolvedBuckets[targetKey];
			originBucket.DependsOnBuckets.Add(targetBucket);
			targetBucket.Dependants.Add(originBucket);

			// and index the link
			unresolvedBuckets[originKey].Links.Add(new LinkNode(refElement, origin, target));
		}

		/// <summary>
		/// Resolves all links or throws an exception.
		/// </summary>
		/// <param name="config">The source configuration.</param>
		/// <param name="configEventLog">A related event log.</param>
		public void ResolveLinks(Config config, IConfigReaderEventLog configEventLog)
		{
			while(unresolvedBuckets.Count > 0)
			{
				// branch depending on whether we encountered a cycle
				LinkBucket currentBucket = FindFirstResolvableBucket();
				if(currentBucket == null)
				{
					throw new LinkCycleException();
				}
				else
				{
					// get the current option (specified by 'nextIndependentBucket')
					Option currentOption = config.GetOption(currentBucket.Section, currentBucket.Option);

					// resolve the current option's links
					foreach(LinkNode link in currentBucket.Links)
					{
						Option targetOption = config.GetOption(link.Target.Section, link.Target.Option);
						currentOption.Elements.Replace(link.RefElement, targetOption.Elements);
					}

					// mark the bucket as resolved
					int currentBucketKey = currentBucket.ToKey();
					unresolvedBuckets.Remove(currentBucketKey);
					resolvedBuckets.Add(currentBucketKey, currentBucket);
				}
			}
		}

		/// <summary>
		/// Determines whether the specified bucket has been resolved.
		/// </summary>
		/// <returns><c>true</c> if the specified bucket has been resolved; otherwise, <c>false</c>.</returns>
		/// <param name="bucket">The bucket.</param>
		internal bool IsBucketResolved(LinkBucket bucket)
		{
			return resolvedBuckets.ContainsKey(bucket.ToKey());
		}

		/// <summary>
		/// Finds the first resolvable bucket.
		/// </summary>
		/// <returns>The first resolvable bucket.</returns>
		protected LinkBucket FindFirstResolvableBucket()
		{
			foreach(LinkBucket bucket in unresolvedBuckets.Values)
			{
				if(bucket.IsReadyToBeResolved(this))
				{
					return bucket;
				}
			}
			return null;
		}
	}
}
