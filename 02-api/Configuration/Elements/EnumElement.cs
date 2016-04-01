using System;
using System.Collections.Generic;
using Ini.Backlogs;
using Ini.Schema;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// The element of the <see cref="Enum" /> type.
    /// </summary>
    public class EnumElement : Element<string>
    {
        #region Properties

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public TEnum GetEnumValue<TEnum>()
            where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("The TEnum must be an enum.");
            }

            return (TEnum)Enum.Parse(typeof(TEnum), Value);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration element.
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
