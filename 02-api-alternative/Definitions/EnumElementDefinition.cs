using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Definitions
{
    /// <summary>
    /// The definition for an enum element.
    /// </summary>
    /// <typeparam name="TEnum">The enumeration type, must be enum.</typeparam>
    public class EnumElementDefinition<TEnum> : ElementDefinition
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        #region Constructor

        /// <summary>
        /// Initializes the <see cref="EnumElementDefinition{TEnum}"/> class.
        /// </summary>
        static EnumElementDefinition()
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("The TEnum must be an enum.");
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration element definition.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
