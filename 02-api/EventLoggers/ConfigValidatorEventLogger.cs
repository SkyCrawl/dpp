using System;
using System.IO;

namespace Ini.EventLoggers
{
	/// <summary>
    /// An implementation of <see cref="IConfigValidatorEventLogger"/> that writes a text writer.
	/// </summary>
    public class ConfigValidatorEventLogger : IConfigValidatorEventLogger
	{
        /// <summary>
        /// The output stream to write event logs to.
        /// </summary>
        protected TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigValidatorEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public ConfigValidatorEventLogger(TextWriter writer)
        {
            this.writer = writer;
        }

        #region IConfigValidationBase implementation

        /// <summary>
        /// Configuration can not be validated without a specification.
        /// </summary>
        public virtual void NoSpecification()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Configuration can not be validated without a valid specification.
        /// </summary>
        public virtual void SpecificationNotValid()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IConfigValidatorEventLogger implementation

        /// <summary>
        /// Specification for the given section was not found when validating configuration.
        /// </summary>
        /// <param name="identifier">The section's identifier.</param>
        public virtual void MissingSectionSpecification(string identifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Specification for the given option was not found when validating configuration.
        /// </summary>
        /// <param name="identifier">The option's identifier.</param>
        public virtual void MissingOptionSpecification(string identifier)
        {
            throw new NotImplementedException();
        }

        #endregion
	}
}
