using System;
using System.Collections.Generic;
using System.Linq;
using Ini.Configuration;
using Ini.Exceptions;
using Ini.EventLoggers;
using Ini.Configuration.Base;

namespace Ini.Util.LinkResolving
{
	/// <summary>
	/// Dependency "graph" implementation for configuration links.
	/// The general contract is to add links as long as needed, and then
	/// resolve them.
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

        #region Public interface

		/// <summary>
		/// Adds the specified link node into the "graph" and updates dependencies.
		/// </summary>
        /// <param name="node">The node.</param>
        public void AddLink(LinkNode node)
		{
			// just in case checks
            if(!node.Origin.IsKeySourceValid())
			{
				throw new ArgumentException("Could not determine the link's origin key because the source data (section or option name) is invalid.");
			}
            if(!node.Target.IsKeySourceValid())
			{
                throw new ArgumentException("Could not determine the link's target key because the source data (section or option name) is invalid.");
			}

			// index the link's origin bucket
            int originKey = node.Origin.ToKey();
			if(!unresolvedBuckets.ContainsKey(originKey))
			{
                unresolvedBuckets[originKey] = new LinkBucket(node.Origin.Section, node.Origin.Option);
			}

			// index the link's target bucket
            int targetKey = node.Target.ToKey();
			if(!unresolvedBuckets.ContainsKey(targetKey))
			{
                unresolvedBuckets[targetKey] = new LinkBucket(node.Target.Section, node.Target.Option);
			}

            // index the link
			LinkBucket originBucket = unresolvedBuckets[originKey];
            originBucket.Links.Add(node);

            // update the dependency graph
            LinkBucket targetBucket = unresolvedBuckets[targetKey];
			originBucket.DependsOnBuckets.Add(targetBucket);
			targetBucket.Dependants.Add(originBucket);
		}

		/// <summary>
		/// Resolves all links or throws an exception.
		/// </summary>
		/// <param name="config">The source configuration.</param>
		/// <param name="configEventLog">A related event log.</param>
        /// <exception cref="Ini.Exceptions.LinkCycleException">If there is a dependency cycle.</exception>
		public void ResolveLinks(Config config, IConfigReaderEventLogger configEventLog)
		{
			while(unresolvedBuckets.Count > 0)
			{
				// branch depending on whether we encountered a cycle
				LinkBucket currentBucket = FindABucketToResolve();
				if(currentBucket == null)
				{
					throw new LinkCycleException();
				}
				else
				{
                    // resolve the currently processed bucket's links
					foreach(LinkNode link in currentBucket.Links)
					{
                        link.LinkElement.Resolve(config, configEventLog);
					}

					// mark the bucket as resolved
                    int currentBucketKey = currentBucket.ToKey();
					unresolvedBuckets.Remove(currentBucketKey);
					resolvedBuckets.Add(currentBucketKey, currentBucket);
				}
			}
		}

        #endregion

        #region Internal interface

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
		/// Finds the first bucket that can be resolved.
		/// </summary>
		/// <returns>The first resolvable bucket, or null.</returns>
		protected LinkBucket FindABucketToResolve()
		{
            return unresolvedBuckets.Values.FirstOrDefault(bucket => bucket.IsReadyToBeResolved(this));
		}

        #endregion
	}
}
