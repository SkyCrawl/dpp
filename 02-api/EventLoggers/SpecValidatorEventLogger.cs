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
        /// <param name="sectionIdentifier">Containing section's identifier.</param>
        public virtual void DuplicateSection(string sectionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A duplicate option within a section has been found in the associated specification.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        public virtual void DuplicateOption(string sectionIdentifier, string optionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The given option is mandatory but it doesn't define any default value.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        public virtual void NoValue(string sectionIdentifier, string optionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The given option was declared as single-value but defined multiple default values.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        public virtual void TooManyValues(string sectionIdentifier, string optionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The given enumeration option didn't define enough allowed values.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        public virtual void NoEnumValues(string sectionIdentifier, string optionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The given option defined a default value that was not listed as allowed.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueNotAllowed(string sectionIdentifier, string optionIdentifier, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The given option defined a default value that was out of range.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueOutOfRange(string sectionIdentifier, string optionIdentifier, object value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
