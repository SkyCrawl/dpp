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
    /// Element of type <see cref="double"/>.
    /// </summary>
    public class DoubleValue : ValueBase<double>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleValue"/> class.
        /// </summary>
        internal DoubleValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleValue"/> class
        /// with an initial value.
        /// </summary>
        public DoubleValue(double value) : base(value)
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
            this.Value = Double.Parse(value);
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
            DoubleOptionSpec doubleSpec = specification as DoubleOptionSpec;
            if(Value < doubleSpec.MinValue || Value > doubleSpec.MaxValue)
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
