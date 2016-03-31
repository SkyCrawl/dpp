using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Definitions;
using IniConfiguration.Interfaces;
using IniConfiguration.Schema;

namespace IniConfiguration.Elements
{
    /// <summary>
    /// The element of the <see cref="Enum" /> type.
    /// </summary>
    public class EnumElement<TEnum> : Element<TEnum>
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="definition"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(ValidationMode mode, OptionDefinition definition, IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
