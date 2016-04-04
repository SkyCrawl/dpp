using System;

namespace Ini.Util
{
    /// <summary>
    /// The identifier-based sort order to apply before saving a configuration.
    /// </summary>
    public enum ConfigBlockSortOrder
    {
        /// <summary>
		/// Same order as the origin (e.g. file).
        /// </summary>
        Insertion,

        /// <summary>
        /// Same order as in the associated schema.
        /// </summary>
        Schema,

        /// <summary>
        /// Top to bottom, lexicographically lowest to highest.
        /// </summary>
        Ascending,

		/// <summary>
		/// Top to bottom, lexicographically highest to lowest.
		/// </summary>
		Descending
    }
}
