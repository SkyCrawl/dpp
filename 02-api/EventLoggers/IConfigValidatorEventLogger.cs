using System;

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
        void MissingSectionSpecification(string identifier);

        /// <summary>
        /// Specification for the given option was not found when validating configuration.
        /// </summary>
        /// <param name="identifier">The option's identifier.</param>
        void MissingOptionSpecification(string identifier);

        // TODO:
	}
}
