using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
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
        /// Parses the string value and initializes the <see cref="ValueBase{T}.Value"/> property.
        /// </summary>
        /// <param name="value">The string.</param>
        public override void FillFromString(string value)
        {
            this.Value = IniSyntax.TrimWhitespaces(value);
        }

        /// <summary>
        /// Serializes this element into a string that can be deserialized back using <see cref="ConfigParser"/>.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <returns>The element converted to a string.</returns>
        public override string ToOutputString(Config config)
        {
            return this.Value;
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
