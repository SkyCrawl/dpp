using System;
using System.Collections.Generic;
using Ini.Specification;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface that defines events for specification validation.
    /// </summary>
    public interface ISpecValidatorEventLogger
    {
        /// <summary>
        /// A duplicate section has been found in the associated specification.
        /// </summary>
        /// <param name="identifier">Containing section's identifier.</param>
        void DuplicateSection(string identifier);

        /// <summary>
        /// A duplicate option within a section has been found in the associated specification.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        void DuplicateOption(string section, string option);

        /// <summary>
        /// The given option is optional but it doesn't define any default value.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        void NoValue(string section, string option);

        /// <summary>
        /// The given option was declared as single-value but defined multiple default values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        void TooManyValues(string section, string option);

        /// <summary>
        /// The given enumeration option didn't define enough allowed values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        void NoEnumValues(string section, string option);

        /// <summary>
        /// The given option defined a default value that was not listed as allowed.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        void ValueNotAllowed(string section, string option, object value);

        /// <summary>
        /// The given option defined a default value that was out of range.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        void ValueOutOfRange(string section, string option, object value);
    }
}
