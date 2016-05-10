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
    /// Element of type <see cref="bool"/>.
    /// </summary>
    public class BoolValue : ValueBase<bool>
    {
        #region Properties

        /// <summary>
        /// The strings that represent "true" value.
        /// </summary>
        protected HashSet<string> trueStrings = new HashSet<string>()
        {
            "1", "t", "y", "on", "yes", "enabled"
        };

        /// <summary>
        /// The strings that represent "false" value.
        /// </summary>
        protected HashSet<string> falseStrings = new HashSet<string>()
        {
            "0", "f", "n", "off", "no", "disabled"
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolValue"/> class.
        /// </summary>
        internal BoolValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolValue"/> class
        /// with an initial value.
        /// </summary>
        public BoolValue(bool value) : base(value)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates an instance of self from the given string value.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <exception cref="ArgumentException">If the given string value could not be interpreted.</exception>
        public override void FillFromString(string value)
        {
            // the library doesn't care about casing, as long as the base string is matched
            string lowercaseValue = value.ToLower();
            if(trueStrings.Contains(lowercaseValue))
            {
                this.Value = true;
            }
            else if(falseStrings.Contains(lowercaseValue))
            {
                this.Value = false;
            }
            else
            {
                throw new ArgumentException("Unknown boolean representation: " + value);
            }
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
