using System;
using System.Collections.Generic;
using System.IO;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="ISpecValidatorEventLogger"/> that writes a text writer.
    /// </summary>
    public class SpecValidatorEventLogger : ISpecValidatorEventLogger
    {
        /// <summary>
        /// The output stream to write event logs to.
        /// </summary>
        protected TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecValidatorEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public SpecValidatorEventLogger(TextWriter writer)
        {
            this.writer = writer;
        }
            
        #region ISpecValidatorEventLogger implementation

        /// <summary>
        /// A duplicate section has been found in the associated specification.
        /// </summary>
        /// <param name="section">Containing section's identifier.</param>
        public virtual void DuplicateSection(string section)
        {
            this.writer.WriteLine(string.Format("ERROR: duplicate section '{0}'.", section));
        }

        /// <summary>
        /// A duplicate option within a section has been found in the associated specification.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void DuplicateOption(string section, string option)
        {
            this.writer.WriteLine(string.Format("ERROR: duplicate option '{0}' in section '{1}'.", option, section));
        }

        /// <summary>
        /// The given option is mandatory but it doesn't define any default value.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void NoValue(string section, string option)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' is mandatory but it doesn't define any default value.", option, section));
        }

        /// <summary>
        /// The given option was declared as single-value but defined multiple default values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void TooManyValues(string section, string option)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' is declared as single-value but instead it defines multiple default values.", option, section));
        }

        /// <summary>
        /// The given enumeration option didn't define enough allowed values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void NoEnumValues(string section, string option)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' doesn't define enough enumeration values.", option, section));
        }

        /// <summary>
        /// The given option defined a default value that was not listed as allowed.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueNotAllowed(string section, string option, object value)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' contains a default value ('{2}') that is not explicitly allowed.", option, section, value));
        }

        /// <summary>
        /// The given option defined a default value that was out of range.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueOutOfRange(string section, string option, object value)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' contains a default value ('{2}') that is out of range.", option, section, value));
        }

        #endregion
    }
}
