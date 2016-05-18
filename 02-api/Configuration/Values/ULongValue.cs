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
    /// Element of type <see cref="ulong"/>.
    /// </summary>
    public class ULongValue : ValueBase<ulong>
    {
        #region Properties

        /// <summary>
        /// The original number base as determined by <see cref="FillFromString"/>,
        /// or user-defined number base. It is used for serialization. Default: decimal.
        /// </summary>
        /// <value>The base.</value>
        public NumberBase Base { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ULongValue"/> class.
        /// </summary>
        internal ULongValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ULongValue"/> class
        /// with an initial value.
        /// </summary>
        public ULongValue(ulong value) : base(value)
        {
            this.Base = NumberBase.DECIMAL;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parses the string value and initializes the <see cref="ValueBase{T}.Value"/> property.
        /// </summary>
        /// <param name="value">The string.</param>
        public override void FillFromString(string value)
        {
            value = value.Trim();

            // first determine the base
            if(value.StartsWith("0x"))
            {
                this.Base = NumberBase.HEXADECIMAL;
                value = value.Substring(2);
            }
            else if(value.StartsWith("0b"))
            {
                this.Base = NumberBase.BINARY;
                value = value.Substring(2);
            }
            else if(value.StartsWith("0"))
            {
                this.Base = NumberBase.OCTAL;
                value = value.Substring(1);
            }
            else
            {
                this.Base = NumberBase.DECIMAL;
            }

            // and then parse
            this.Value = Convert.ToUInt64(value, this.Base.ToBase());
        }

        /// <summary>
        /// Serializes this element into a string that can be deserialized back using <see cref="ConfigParser"/>.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <returns>The element converted to a string.</returns>
        public override string ToOutputString(Config config)
        {
            string prefix = null;
            switch(Base)
            {
                case NumberBase.BINARY:
                    prefix = "0b";
                    break;
                case NumberBase.OCTAL:
                    prefix = "0";
                    break;
                case NumberBase.DECIMAL:
                    prefix = "";
                    break;
                case NumberBase.HEXADECIMAL:
                    prefix = "0x";
                    break;
                default:
                    throw new ArgumentException("Unknown number base: " + Base.ToString());
            }

            // see: http://stackoverflow.com/a/6986103
            return prefix + Convert.ToString((long) this.Value, Base.ToBase());
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
            ULongOptionSpec uLongSpec = specification as ULongOptionSpec;
            if(Value < uLongSpec.MinValue || Value > uLongSpec.MaxValue)
            {
                configLogger.ValueOutOfRange(
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
