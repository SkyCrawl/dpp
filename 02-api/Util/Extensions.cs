using System;
using System.Collections.Generic;
using System.Linq;
using Ini.Configuration;
using Ini.Configuration.Base;
using Ini.Specification;

namespace Ini.Util
{
    /// <summary>
    /// This library's proprietary extensions. Nah, just kidding...
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns value from a dictionary for a specified key. If the dictionary does not contain the key, returns default value.
        /// </summary>
        /// <typeparam name="KeyType">The key type.</typeparam>
        /// <typeparam name="ValueType">The value type.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key that identifies the searched value.</param>
        /// <returns>The search result or a default value.</returns>
        public static ValueType TryGetValue<KeyType, ValueType>(this IDictionary<KeyType, ValueType> dictionary, KeyType key)
        {
            if(dictionary == null)
            {
                return default(ValueType);
            }

            ValueType result;
            dictionary.TryGetValue(key, out result);
            return result;
        }

        /// <summary>
        /// Gets a collection of keys mapped to values that are equal to the specified value.
        /// </summary>
        /// <returns>The collection.</returns>
        /// <param name="dictionary">The current dictionary.</param>
        /// <param name="value">The specified value.</param>
        /// <typeparam name="TKey">Key type for the dictionary.</typeparam>
        /// <typeparam name="TValue">Value type for the dictionary.</typeparam>
        public static IEnumerable<TKey> GetKeysForValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            List<TKey> result = new List<TKey>();
            foreach(KeyValuePair<TKey, TValue> entry in dictionary)
            {
                if(entry.Value.Equals(value))
                {
                    result.Add(entry.Key);
                }
            }
            return result;
        }

        /// <summary>
        /// Converts this enumerable of object values into an array of correctly typed elementary values.
        /// </summary>
        /// <returns>The array.</returns>
        /// <param name="enumerable">The source enumerable.</param>
        /// <typeparam name="OutputType">The correct elementary type.</typeparam>
        /// <exception cref="InvalidCastException">The specified type was incorrect.</exception>
        public static OutputType[] GetValues<OutputType>(this IEnumerable<IValue> enumerable)
        {
            List<OutputType> result = new List<OutputType>();
            foreach(IValue value in enumerable)
            {
                result.Add(value.GetValue<OutputType>());
            }
            return result.ToArray();
        }

        /// <summary>
        /// Reorders this dictionary's values by the given order while preserving relative positions
        /// of commentary blocks.
        /// </summary>
        /// <param name="configItems">The dictionary whose values are to be reordered.</param>
        /// <param name="specItems">The specification items to search for if a particular order is applied.</param>
        /// <param name="order">The order to apply.</param>
        /// <returns>The reordered values.</returns>
        public static IEnumerable<ConfigBlockBase> ReorderBlocks(this ObservableInsertionDictionary<string, ConfigBlockBase> configItems, IEnumerable<SpecBlockBase> specItems, ConfigBlockSortOrder order)
        {
            if (configItems == null)
                throw new ArgumentNullException("configItems");

            // Comments are groupped to the next non-comment elements.
            var result = new List<ConfigBlockBase>();
            var groupsResult = GroupBlocks(configItems.Values);

            switch (order)
            {
                case ConfigBlockSortOrder.Ascending:
                    result.AddRange(groupsResult.Groups.OrderBy(item => item.Identifier).SelectMany(item => item.Blocks));
                    break;
                case ConfigBlockSortOrder.Descending:
                    result.AddRange(groupsResult.Groups.OrderByDescending(item => item.Identifier).SelectMany(item => item.Blocks));
                    break;
                case ConfigBlockSortOrder.Insertion:
                    return configItems.Values;
                case ConfigBlockSortOrder.Specification:
                    // Special case for sections, that are not in the configuration.
                    if (specItems == null)
                        return configItems.Values;

                    var itemsToWrite = groupsResult.Groups.ToDictionary(item => item.Identifier, item => item.Blocks);

                    // First write groups, that are in the specification.
                    foreach (var specIdentifier in specItems.Select(item => item.Identifier))
                    {
                        List<ConfigBlockBase> blocks;
                        if (itemsToWrite.TryGetValue(specIdentifier, out blocks))
                        {
                            itemsToWrite.Remove(specIdentifier);
                            result.AddRange(blocks);
                        }
                    }

                    // Next write groups, that were not in the specification.
                    result.AddRange(itemsToWrite.SelectMany(item => item.Value));
                    break;
                default:
                    throw new NotImplementedException(string.Format("Unknown type of sort order: {0}.", order));
            }

            // The trailing comments are appended to the end.
            result.AddRange(groupsResult.TrailingComments);

            return result;
        }

        static BlocksGroups GroupBlocks(IEnumerable<ConfigBlockBase> blocks)
        {
            var result = new BlocksGroups();
            BlocksGroup group = new BlocksGroup();

            foreach (var block in blocks)
            {
                group.Blocks.Add(block);

                if (!(block is Commentary))
                {
                    group.Identifier = block.Identifier;
                    result.Groups.Add(group);
                    group = new BlocksGroup();
                }
            }

            result.TrailingComments.AddRange(group.Blocks);

            return result;
        }

        
        /// <summary>
        /// Converts this value into the actual number base represented.
        /// </summary>
        /// <returns>The number base.</returns>
        /// <param name="numberBase">The source enum value.</param>
        /// <exception cref="ArgumentException">The given value is not handled in this method.</exception>
        public static int ToBase(this NumberBase numberBase)
        {
            switch(numberBase)
            {
                case NumberBase.BINARY:
                    return 2;
                case NumberBase.OCTAL:
                    return 8;
                case NumberBase.DECIMAL:
                    return 10;
                case NumberBase.HEXADECIMAL:
                    return 16;
                default:
                    throw new ArgumentException("Unknown number base: " + numberBase.ToString());
            }
        }

        #region Private Classes

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
