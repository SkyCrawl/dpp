using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Ini.Configuration.Base;
using System.IO;
using Ini.Configuration;
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
        /// <param name="items">The dictionary whose values are to be reordered.</param>
        /// <param name="specItems">The specification items to search for if a particular order is applied.</param>
        /// <param name="order">The order to apply.</param>
        /// <returns>The reordered values.</returns>
        public static IEnumerable<ConfigBlockBase> ReorderBlocks(this ObservableInsertionDictionary<string, ConfigBlockBase> items, IEnumerable<SpecBlockBase> specItems, ConfigBlockSortOrder order)
        {
            // first handle the trivial case
            if(order == ConfigBlockSortOrder.Insertion)
            {
                return items.Values;
            }
            else
            {
                // create a mapping of non-commentary configuration blocks to commentary configuration blocks
                Dictionary<ConfigBlockBase, ConfigBlockBase> commentaryAnchoringBlocks = items.Values.AnchorComments();

                // filter commentaries
                IEnumerable<ConfigBlockBase> nonCommentaryBlocks = items.Values.Where(item => !(item is Commentary));

                // order non-commentary blocks by the given sort order
                IEnumerable<ConfigBlockBase> orderedNonCommentaryBlocks = null;
                switch(order)
                {
                case ConfigBlockSortOrder.Ascending:
                    orderedNonCommentaryBlocks = nonCommentaryBlocks.OrderBy(item => item.Identifier);
                    break;
                case ConfigBlockSortOrder.Descending:
                    orderedNonCommentaryBlocks = nonCommentaryBlocks.OrderByDescending(item => item.Identifier);
                    break;
                case ConfigBlockSortOrder.Insertion:
                    orderedNonCommentaryBlocks = nonCommentaryBlocks;
                    break;
                case ConfigBlockSortOrder.Specification:
                    orderedNonCommentaryBlocks = new List<ConfigBlockBase>();
                    foreach(SpecBlockBase specItem in specItems)
                    {
                        if(items.ContainsKey(specItem.Identifier))
                        {
                            (orderedNonCommentaryBlocks as List<ConfigBlockBase>).Add(items[specItem.Identifier]);
                        }
                        else
                        {
                            /*
                         * No need to throw an exception:
                         * - The contract of this method is not to call it before the configuration is validated.
                         * - If the configuration is validated using strict mode, no mandatory sections will be missing.
                         * - If the configuration is validated using relaxed mode (for some reason), we don't really have to care.
                         */
                        }
                    }
                    break;
                default:
                    throw new ArgumentException("Unknown enum value: " + order.ToString());
                }

                // join commentaries and ordered non-commentary blocks in the right way
                List<ConfigBlockBase> result = new List<ConfigBlockBase>();
                foreach(ConfigBlockBase block in orderedNonCommentaryBlocks)
                {
                    if(commentaryAnchoringBlocks.ContainsKey(block))
                    {
                        // first add the corresponding commentary
                        result.Add(commentaryAnchoringBlocks[block]);
                    }

                    // and then always add the current block
                    result.Add(block);
                }

                // and finally, return
                return result;
            }
        }

        /// <summary>
        /// Anchors each commentary block to the immediately following block.
        /// </summary>
        /// <returns>Mapping of non-commentary blocks that anchor a commentary block.</returns>
        /// <param name="items">The source blocks.</param>
        public static Dictionary<ConfigBlockBase, ConfigBlockBase> AnchorComments(this IEnumerable<ConfigBlockBase> items)
        {
            Dictionary<ConfigBlockBase, ConfigBlockBase> result = new Dictionary<ConfigBlockBase, ConfigBlockBase>();
            ConfigBlockBase lastCommentary = null;
            foreach(ConfigBlockBase block in items)
            {
                if(block is Commentary)
                {
                    if(lastCommentary != null)
                    {
                        // join commentaries
                        (lastCommentary as Commentary).Lines.AddRange((block as Commentary).Lines);
                    }
                    else
                    {
                        lastCommentary = block;
                    }
                }
                else if(lastCommentary != null)
                {
                    result.Add(block, lastCommentary);
                    lastCommentary = null;
                }
                else
                {
                    // do nothing as there is no commentary to be anchored by this block
                }
            }

            // and return
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
    }
}
