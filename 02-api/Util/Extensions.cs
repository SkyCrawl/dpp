﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Ini.Configuration.Base;

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
