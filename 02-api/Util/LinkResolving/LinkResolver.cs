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
		/// Adds a link with the specified origin and target into the graph
		/// and updates dependencies.
		/// </summary>
		/// <param name="refElement">The reference element to replace when the link is resolved.</param>
		/// <param name="origin">Link origin.</param>
		/// <param name="target">Link target.</param>
        public void AddLink(IValue refElement, LinkOrigin origin, LinkTarget target)
		{
			// first some precondition checks
			if(!origin.IsKeySourceValid())
			{
				throw new ArgumentException("Could not determine the link's origin key because the source data (section or option name) is invalid.");
			}
			if(!target.IsKeySourceValid())
			{
                throw new ArgumentException("Could not determine the link's target key because the source data (section or option name) is invalid.");
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

            // index the link
			LinkBucket originBucket = unresolvedBuckets[originKey];
            originBucket.Links.Add(new LinkNode(refElement, origin, target));

            // and update the dependency graph
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
					// get the option referenced by the currently processed bucket
                    Option originOption = config.GetOption(currentBucket.Section, currentBucket.Option);

                    // resolve the currently processed bucket's links
					foreach(LinkNode link in currentBucket.Links)
					{
						Option targetOption = config.GetOption(link.Target.Section, link.Target.Option);
						originOption.Values.Replace(link.RefElement, targetOption.Values);
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
