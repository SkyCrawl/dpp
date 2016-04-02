using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.Util
{
    /// <summary>
    /// The sort order of sections and options in saved file.
    /// </summary>
    public enum SortOrder
    {
        /// <summary>
        /// Same as the original file.
        /// </summary>
        InsertionOrder,

        /// <summary>
        /// Same order as in schema.
        /// </summary>
        SchemaOrder,

        /// <summary>
        /// 
        /// </summary>
        IdentifierAscendingOrder,

		/// <summary>
		/// 
		/// </summary>
		IdentifierDescendingOrder
    }
}
