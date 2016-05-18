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
        /// Parses the string value and initializes the <see cref="ValueBase{T}.Value"/> property.
        /// </summary>
        /// <param name="value">The string.</param>
        public override void FillFromString(string value)
        {
            value = value.Trim();

            this.Value = Double.Parse(value);
        }

        /// <summary>
        /// Serializes this element into a string that can be deserialized back using <see cref="ConfigParser"/>.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <returns>The element converted to a string.</returns>
        public override string ToOutputString(Config config)
        {
            return this.Value.ToString();
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
