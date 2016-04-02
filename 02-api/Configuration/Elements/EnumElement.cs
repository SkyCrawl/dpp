﻿using System;
using System.Collections.Generic;
using Ini.Backlogs;
using Ini.Schema;
using Ini.Util;

namespace Ini.Configuration.Elements
{
    /// <summary>
	/// Element for all <see cref="Enum"/> types.
    /// </summary>
    public class EnumElement : Element<string>
    {
        #region Properties

        /// <summary>
		/// Tries to convert <see aref="Value"/> to a value of the given
		/// <see cref="Enum"/> type.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public TEnum GetEnumValue<TEnum>()
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
			if(!typeof(TEnum).IsEnum)
			{
				throw new ArgumentException("The parameter type must be an enum.");
			}
			else
			{
				return (TEnum) Enum.Parse(typeof(TEnum), Value);
			}
        }

        #endregion

		#region Validation

        /// <summary>
		/// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="definition"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(ValidationMode mode, OptionSpec definition, IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
