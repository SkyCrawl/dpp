using System;
using System.Collections.Generic;
using Ini.Specification;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface providing information about validating specifications.
    /// </summary>
    public interface ISpecValidatorEventLogger
    {
        /// <summary>
        /// A duplicate section has been found in the associated specification.
        /// </summary>
        /// <param name="sectionIdentifier">Containing section's identifier.</param>
        void DuplicateSection(string sectionIdentifier);

        /// <summary>
        /// A duplicate option within a section has been found in the associated specification.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        void DuplicateOption(string sectionIdentifier, string optionIdentifier);

        /// <summary>
        /// Mandatory option have to define at least one default value.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        void NoDefaultValue(string sectionIdentifier, string optionIdentifier);

        /// <summary>
        /// The given option was declared as single-value but defined multiple default values.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        void TooManyValues(string sectionIdentifier, string optionIdentifier);

        /// <summary>
        /// The given enumeration option didn't define enough allowed values.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        void MissingEnumValues(string sectionIdentifier, string optionIdentifier);

        /// <summary>
        /// The given option defined a default value that was not listed as allowed.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        void ValueNotAllowed(string sectionIdentifier, string optionIdentifier, object value);

        /// <summary>
        /// The given option defined a default value that was out of range.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        void ValueOutOfRange(string sectionIdentifier, string optionIdentifier, object value);
    }
}
