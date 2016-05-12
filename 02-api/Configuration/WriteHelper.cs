using Ini.Configuration.Base;
using Ini.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.Configuration
{
    /// <summary>
    /// Helper class with common methods used for writing configuration.
    /// </summary>
    public static class WriteHelper
    {
        #region Methods

        /// <summary>
        /// Orders a dictionary of config blocks by specified way.
        /// </summary>
        /// <param name="configItems">Items to be sorted.</param>
        /// <param name="sortOrder">Type of sorting to be used.</param>
        /// <param name="specOrder">Order of blocks in the specification.</param>
        /// <returns>Returns sorted config blocks.</returns>
        /// <exception cref="ArgumentNullException">The config items dictionary is null.</exception>
        /// <exception cref="NotImplementedException">The requested sort order is not implemented.</exception>
        internal static IEnumerable<ConfigBlockBase> GetOrderedItems(this IEnumerable<ConfigBlockBase> configItems, ConfigBlockSortOrder sortOrder, IEnumerable<string> specOrder)
        {
            if (configItems == null)
                throw new ArgumentNullException("configItems");

            var result = new List<ConfigBlockBase>();
            var groupsResult = GroupBlocks(configItems);

            switch (sortOrder)
            {
                case ConfigBlockSortOrder.Ascending:
                    result.AddRange(groupsResult.Groups.OrderBy(item => item.Identifier).SelectMany(item => item.Blocks));
                    break;
                case ConfigBlockSortOrder.Descending:
                    result.AddRange(groupsResult.Groups.OrderByDescending(item => item.Identifier).SelectMany(item => item.Blocks));
                    break;
                case ConfigBlockSortOrder.Insertion:
                    return configItems;
                case ConfigBlockSortOrder.Specification:
                    var itemsToWrite = groupsResult.Groups.ToDictionary(item => item.Identifier, item => item.Blocks);

                    foreach (var specIdentifier in specOrder)
                    {
                        List<ConfigBlockBase> blocks;
                        if (itemsToWrite.TryGetValue(specIdentifier, out blocks))
                        {
                            itemsToWrite.Remove(specIdentifier);
                            result.AddRange(blocks);
                        }
                    }

                    result.AddRange(itemsToWrite.SelectMany(item => item.Value));
                    break;
                default:
                    throw new NotImplementedException(string.Format("Unknown type of sort order: {0}.", sortOrder));
            }

            result.AddRange(groupsResult.TrailingComments);

            return result;
        }

        static BlocksGroups GroupBlocks(IEnumerable<ConfigBlockBase> blocks)
        {
            var result = new BlocksGroups();
            BlocksGroup group = new BlocksGroup();

            foreach(var block in blocks)
            {
                group.Blocks.Add(block);

                if (!(block is Commentary))
                {
                    block.Identifier = block.Identifier;
                    result.Groups.Add(group);
                    group = new BlocksGroup();
                }
            }

            result.TrailingComments.AddRange(group.Blocks);

            return result;
        }

        #endregion

        #region Classes

        class BlocksGroups
        {
            #region Properties

            public List<BlocksGroup> Groups { get; private set; }

            public List<ConfigBlockBase> TrailingComments { get; private set; }

            #endregion

            #region Constructor

            public BlocksGroups()
            {
                Groups = new List<BlocksGroup>();
                TrailingComments = new List<ConfigBlockBase>();
            }

            #endregion
        }

        class BlocksGroup
        {
            #region Properties

            public string Identifier { get; set; }

            public List<ConfigBlockBase> Blocks { get; private set; }

            #endregion

            #region Constructor

            public BlocksGroup()
            {
                Blocks = new List<ConfigBlockBase>();
            }

            #endregion
        }

        #endregion
    }
}
