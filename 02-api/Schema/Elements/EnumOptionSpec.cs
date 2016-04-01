using System;
using System.Collections.Generic;

namespace Ini.Schema.Elements
{
    /// <summary>
    /// The definition for an enum option.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type, must be enum.</typeparam>
    public class EnumOptionSpec<TEnum> : OptionSpec<TEnum>
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        #region Constructor

        /// <summary>
		/// Initializes the <see cref="EnumOptionSpec{TEnum}"/> class.
        /// </summary>
        static EnumOptionSpec()
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("The TEnum must be an enum.");
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
