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
    /// Element of type <see cref="long"/>.
    /// </summary>
    public class LongValue : ValueBase<long>
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
        /// Initializes a new instance of the <see cref="LongValue"/> class.
        /// </summary>
        internal LongValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongValue"/> class
        /// with an initial value.
        /// </summary>
        public LongValue(long value) : base(value)
        {
            this.Base = NumberBase.DECIMAL;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates an instance of self from the given string value.
        /// </summary>
        /// <param name="value">The string.</param>
        public override void FillFromString(string value)
        {
            // first determine the base
            if(value.StartsWith("0x"))
            {
                this.Base = NumberBase.HEXADECIMAL;
            }
            else if(value.StartsWith("0b"))
            {
                this.Base = NumberBase.BINARY;
            }
            else if(value.StartsWith("0"))
            {
                this.Base = NumberBase.OCTAL;
            }
            else
            {
                this.Base = NumberBase.DECIMAL;
            }

            // and then parse
            this.Value = Convert.ToInt64(value, this.Base.ToBase());
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
            LongOptionSpec longSpec = specification as LongOptionSpec;
            if(Value < longSpec.MinValue || Value > longSpec.MaxValue)
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
