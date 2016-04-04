using System;
using System.Collections.Generic;

namespace Ini.EventLogs
{
    /// <summary>
    /// An implementation of <see cref="ISpecValidatorEventLog"/> that writes into the console.
    /// </summary>
    public class ConsoleSchemaValidatorEventLog : ISpecValidatorEventLog
    {
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
