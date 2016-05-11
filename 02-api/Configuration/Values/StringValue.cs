using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Configuration.Base;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Element of type <see cref="string"/>.
    /// </summary>
    public class StringValue : ValueBase<string>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StringValue"/> class.
        /// </summary>
        internal StringValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Values.StringValue"/> class.
        /// </summary>
        /// <param name="value">Initial value.</param>
        public StringValue(string value) : base(value)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates an instance of self from the given string value.
        /// </summary>
        /// <param name="value">The string.</param>
        public override void FillFromString(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Converts the inner value into a string with the appropriate format.
        /// </summary>
        /// <returns>The value converted to a string.</returns>
        public override string ToStringFormat()
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
            return true;
        }

        #endregion
    }
}
