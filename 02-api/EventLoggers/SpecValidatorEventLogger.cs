using System;
using System.Collections.Generic;
using System.IO;
using Ini.Properties;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="ISpecValidatorEventLogger"/> forwarding output to the inherited <see cref="BaseEventLogger"/>.
    /// </summary>
    public class SpecValidatorEventLogger : BaseEventLogger, ISpecValidatorEventLogger
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecValidatorEventLogger"/> class.
        /// </summary>
        /// <param name="writer">Output stream for specification validation events.</param>
        public SpecValidatorEventLogger(TextWriter writer)
            : base(writer)
        {
        }

        #endregion

        #region ISpecValidatorEventLogger implementation

        /// <summary>
        /// A duplicate section has been found in the associated specification.
        /// </summary>
        /// <param name="section">Containing section's identifier.</param>
        public virtual void DuplicateSection(string section)
        {
            Writer.WriteLine(Resources.SpecValidationDuplicateSection, section);
        }

        /// <summary>
        /// A duplicate option within a section has been found in the associated specification.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void DuplicateOption(string section, string option)
        {
            Writer.WriteLine(Resources.SpecValidationDuplicateOption, option, section);
        }

        /// <summary>
        /// The given option is optional but it doesn't define any default value.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void NoValue(string section, string option)
        {
            Writer.WriteLine(Resources.SpecValidationNoValue, option, section);
        }

        /// <summary>
        /// The given option was declared as single-value but defined multiple default values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void TooManyValues(string section, string option)
        {
            Writer.WriteLine(Resources.SpecValidationTooManyValues, option, section);
        }

        /// <summary>
        /// The given enumeration option didn't define enough allowed values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void NoEnumValues(string section, string option)
        {
            Writer.WriteLine(Resources.SpecValidationNoEnumValues, option, section);
        }

        /// <summary>
        /// The given option defined a default value that was not listed as allowed.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueNotAllowed(string section, string option, object value)
        {
            Writer.WriteLine(Resources.SpecValidationValueNotAllowed, option, section, value);
        }

        /// <summary>
        /// The given option defined a default value that was out of range.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueOutOfRange(string section, string option, object value)
        {
            Writer.WriteLine(Resources.SpecValidationValueOutOfRange, option, section, value);
        }

        #endregion
    }
}
