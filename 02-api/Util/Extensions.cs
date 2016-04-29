using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Ini.Util
{
	/// <summary>
	/// This library's proprietary extensions. Nah, just kidding...
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Replaces the specified element in the specified list with the specified elements.
		/// </summary>
		/// <param name="list">The list.</param>
        /// <param name="item">The item to replace.</param>
		/// <param name="replacement">The replacement elements.</param>
		/// <typeparam name="TSource">Pretty much anything.</typeparam>
		public static void Replace<TSource>(this IList<TSource> list, TSource item, IEnumerable<TSource> replacement)
		{
			int index = list.IndexOf(item);
			if(index == -1)
			{
				throw new ArgumentException("Can not replace because the element was not found in the collection.");
			}
			else
			{
                // remove the source element
                list.RemoveAt(index);

                // and then insert replacement while preserving order
                foreach(TSource newItem in replacement.Reverse())
                {
                    list.Insert(index, newItem);
                }
			}
		}
	}
}
