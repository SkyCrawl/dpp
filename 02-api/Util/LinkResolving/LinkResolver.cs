using System;
using System.Collections.Generic;
using System.Linq;
using Ini.Configuration;
using Ini.EventLoggers;
using Ini.Exceptions;

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
        /// Adds the specified node into the "graph" and updates dependencies.
        /// The origin and target are assumed to be valid.
        /// </summary>
        /// <param name="node">The node.</param>
        public void AddLink(LinkNode node)
        {
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
            if(originBucket.Links.Contains(node))
            {
                throw new ArgumentException("This node has already been added.");
            }
            originBucket.Links.Add(node);

            // update dependency graph
            LinkBucket targetBucket = unresolvedBuckets[targetKey];
            originBucket.DependsOnBuckets.Add(targetBucket);
            targetBucket.Dependants.Add(originBucket);
        }

        /// <summary>
        /// Removes the specified node from the "graph" and updates dependencies.
        /// The origin and target are assumed to be valid.
        /// </summary>
        /// <param name="node">The node.</param>
        public void RemoveLink(LinkNode node)
        {
            // compute and check the keys
            int originKey = node.Origin.ToKey();
            int targetKey = node.Target.ToKey();
            if(unresolvedBuckets.ContainsKey(originKey) && unresolvedBuckets.ContainsKey(targetKey))
            {
                // get the buckets
                LinkBucket originBucket = unresolvedBuckets[originKey];
                LinkBucket targetBucket = unresolvedBuckets[targetKey];

                // remove the link
                originBucket.Links.Remove(node);
                originBucket.DependsOnBuckets.Remove(targetBucket);
                targetBucket.Dependants.Remove(originBucket);

                // the below method doesn't mind empty buckets so no need to remove them
            }
            else
            {
                throw new ArgumentException("Can not remove a link that has not been added to the resolver.");
            }
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
