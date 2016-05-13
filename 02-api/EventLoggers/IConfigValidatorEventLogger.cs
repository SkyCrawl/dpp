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
        /// The logger used for spec validation.
        /// </summary>
        ISpecValidatorEventLogger SpecValidationLogger { get; }

        /// <summary>
        /// Specification for the given section was not found when validating configuration.
        /// </summary>
        /// <param name="identifier">The section's identifier.</param>
        void NoSectionSpecification(string identifier);

        /// <summary>
        /// Specification for the given option was not found when validating configuration.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        void NoOptionSpecification(string section, string option);

        /// <summary>
        /// The given option's value type does not conform to the specification.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="typeSpec">The option's value type from specification.</param>
        /// <param name="typeOption">The option's value type.</param>
        void ValueTypeMismatch(string section, string option, Type typeSpec, Type typeOption);

        /// <summary>
        /// The given option is mandatory but it doesn't contain a value.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        void NoValue(string section, string option);

        /// <summary>
        /// The given option is declared as single-value but it contains multiple values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        void TooManyValues(string section, string option);

        /// <summary>
        /// The given option contains a link that references a removed option (target).
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="link">The affected link.</param>
        void LinkInconsistent(string section, string option, ILink link);

        /// <summary>
        /// The given option contains a value that is not allowed.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        void ValueNotAllowed(string section, string option, IValue value);

        /// <summary>
        /// The given option contains a value that is out of range.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        void ValueOutOfRange(string section, string option, IValue value);
    }
}
