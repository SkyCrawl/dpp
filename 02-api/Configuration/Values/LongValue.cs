using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using Ini.Configuration.Base;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Element of type <see cref="long"/>.
    /// </summary>
    public class LongValue : ValueBase<long>
    {
        #region Properties

        /// <summary>
        /// The original number base when <see cref="FillFromString"/> method is used,
        /// or the base for serialization. Default: decimal.
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
        /// <param name="optionSpec">The option specification.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="eventLog">The validation event log.</param>
        /// <returns></returns>
        public override bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLogger eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
