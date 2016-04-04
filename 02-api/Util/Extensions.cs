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
		/// <param name="element">The element.</param>
		/// <param name="replacement">The replacement elements.</param>
		/// <typeparam name="TSource">Pretty much anything.</typeparam>
		public static void Replace<TSource>(this Collection<TSource> list, TSource element, IEnumerable<TSource> replacement)
		{
			int index = list.IndexOf(element);
			if(index == -1)
			{
				throw new ArgumentException("Can not replace because the element was not found in the collection.");
			}
			else
			{
				// construct the replacement list
				List<TSource> result = new List<TSource>(list.Take(index));
				result.AddRange(replacement);
				result.AddRange(list.Skip(index + 1));

				// and then replace the original list
				list.Clear();
				foreach(TSource newElement in result)
				{
					list.Add(newElement);
				}
			}
		}
	}
}
