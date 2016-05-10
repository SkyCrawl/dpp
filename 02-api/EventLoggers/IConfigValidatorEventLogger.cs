using System;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
	/// <summary>
    /// Interface defining configuration validation events.
	/// </summary>
    public interface IConfigValidatorEventLogger : IConfigValidationBase
	{
        /// <summary>
        /// Specification for the given section was not found when validating configuration.
        /// </summary>
        /// <param name="identifier">The section's identifier.</param>
        void NoSectionSpecification(string identifier);

        /// <summary>
        /// Specification for the given option was not found when validating configuration.
        /// </summary>
        /// <param name="identifier">The option's identifier.</param>
        void NoOptionSpecification(string identifier);

        /// <summary>
        /// The given option's value type does not conform to the specification.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="specificationType">The option's value type from specification.</param>
        /// <param name="optionType">The option's value type.</param>
        void ValueTypeMismatch(string sectionIdentifier, string optionIdentifier, Type specificationType, Type optionType);

        /// <summary>
        /// The given option is mandatory but it doesn't contain a value.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        void NoValue(string sectionIdentifier, string optionIdentifier);

        /// <summary>
        /// The given option is declared as single-value but it contains multiple values.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        void TooManyValues(string sectionIdentifier, string optionIdentifier);

        /// <summary>
        /// The given option contains a link that references a removed option (target).
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="link">The affected link.</param>
        void LinkInconsistent(string sectionIdentifier, string optionIdentifier, ILink link);

        /// <summary>
        /// The given option contains a value that is not allowed.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        void ValueNotAllowed(string sectionIdentifier, string optionIdentifier, IValue value);

        /// <summary>
        /// The given option contains a value that is out of range.
        /// </summary>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        void ValueOutOfRange(string sectionIdentifier, string optionIdentifier, IValue value);
	}
}
