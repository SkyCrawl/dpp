using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using Ini.Configuration.Base;
using Ini.Specification.Values;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Element for all <see cref="Enum"/> types.
    /// </summary>
    public class EnumValue : ValueBase<string>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValue"/> class.
        /// </summary>
        internal EnumValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValue"/> class
        /// with an initial value.
        /// </summary>
        public EnumValue(string value) : base(value)
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

        /// <summary>
        /// Creates an instance of self from the given string value.
        /// </summary>
        /// <param name="value">The string.</param>
        public override void FillFromString(string value)
        {
            this.Value = value;
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <param name="section">The current section.</param>
        /// <param name="specification">The current option's specification.</param>
        /// <param name="configLogger">Configuration validation event logger.</param>
        /// <returns></returns>
        public override bool IsValid(Config config, string section, OptionSpec specification, IConfigValidatorEventLogger configLogger)
        {
            EnumOptionSpec enumSpec = specification as EnumOptionSpec;
            if(!enumSpec.AllowedValues.Contains(Value))
            {
                configLogger.ValueNotAllowed(
                    section,
                    specification.Identifier,
                    this);
                return false;
            }
            return true;
        }

        #endregion
    }
}
