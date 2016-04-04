using System;
using System.Collections.Generic;
using Ini.EventLogs;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// Element for all <see cref="Enum"/> types.
    /// </summary>
    public class EnumElement : Element<string>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Elements.EnumElement"/> class.
        /// </summary>
        public EnumElement(string value) : base(value)
        {
        }

        #endregion

        #region Public methods

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
        /// <param name="optionSpec"></param>
        /// <param name="eventLog"></param>
        /// <returns></returns>
        public override bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLog eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
