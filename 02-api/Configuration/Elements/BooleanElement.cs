using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// Element of type <see cref="bool"/>.
    /// </summary>
    public class BooleanElement : Element<bool>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanElement"/> class.
        /// </summary>
        public BooleanElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanElement"/> class
        /// with an initial value.
        /// </summary>
        public BooleanElement(bool value) : base(value)
        {
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
