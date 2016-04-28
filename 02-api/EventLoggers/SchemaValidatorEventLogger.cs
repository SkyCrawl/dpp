﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="ISpecValidatorEventLogger"/> that writes a text writer.
    /// </summary>
    public class SchemaValidatorEventLogger : ISpecValidatorEventLogger
    {
        /// <summary>
        /// The output stream to write event logs to.
        /// </summary>
        protected TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaValidatorEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public SchemaValidatorEventLogger(TextWriter writer)
        {
            this.writer = writer;
        }

        #region IValidationBacklog Members

        /// <summary>
        /// A duplicate section has been found in the associated specification.
        /// </summary>
        /// <param name="sectionIdentifier">Containing section's identifier.</param>
        public void DuplicateSection(string sectionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A duplicate option within a section has been found in the associated specification.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        public void DuplicateOption(string sectionIdentifier, string optionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Default value is expected for the given option.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        public void DefaultValueExpected(string sectionIdentifier, string optionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// An option's default value is invalid.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="elementIndex">Index of the affected element.</param>
        /// <param name="value">The affected value.</param>
        public void DefaultValueInvalid(string sectionIdentifier, string optionIdentifier, int elementIndex, object value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
