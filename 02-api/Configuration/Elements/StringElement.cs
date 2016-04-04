using System;
using System.Collections.Generic;
using Ini.EventLogs;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// Element of type <see cref="string"/>.
    /// </summary>
    public class StringElement : Element<string>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StringElement"/> class.
        /// </summary>
        public StringElement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringElement"/> class
        /// with an initial value.
        /// </summary>
        public StringElement(string value) : base(value)
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
        public override bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLog eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
