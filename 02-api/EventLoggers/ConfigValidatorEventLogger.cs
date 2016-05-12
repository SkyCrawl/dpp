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
            this.writer.WriteLine("ERROR: configuration can not be validated because it isn't associated to a specification.");
        }

        /// <summary>
        /// Configuration can not be validated without a valid specification.
        /// </summary>
        public virtual void SpecificationNotValid()
        {
            this.writer.WriteLine("ERROR: configuration can not be validated because its associated specification is not valid.");
        }

        #endregion

        #region IConfigValidatorEventLogger implementation

        /// <summary>
        /// Specification for the given section was not found when validating configuration.
        /// </summary>
        /// <param name="identifier">The section's identifier.</param>
        public virtual void NoSectionSpecification(string identifier)
        {
            this.writer.WriteLine(string.Format("ERROR: validation of section '{0}' was skipped as it wasn't found in the associated specification.", identifier));
        }

        /// <summary>
        /// Specification for the given option was not found when validating configuration.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void NoOptionSpecification(string section, string option)
        {
            this.writer.WriteLine(string.Format("ERROR: validation of option '{0}' in section '{1}' was skipped as it wasn't found in the associated specification.", option, section));
        }

        /// <summary>
        /// The given option's value type does not conform to the specification.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="typeSpec">The option's value type from specification.</param>
        /// <param name="typeOption">The option's value type.</param>
        public virtual void ValueTypeMismatch(string section, string option, Type typeSpec, Type typeOption)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' is of type '{2}'. Expected: '{3}'.", option, section, typeOption.FullName, typeSpec.FullName));
        }

        /// <summary>
        /// The given option is mandatory but it doesn't contain a value.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void NoValue(string section, string option)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' is mandatory but it doesn't contain a value.", option, section));
        }

        /// <summary>
        /// The given option is declared as single-value but it contains multiple values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void TooManyValues(string section, string option)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' is declared as single-value but instead it contains multiple values.", option, section));
        }

        /// <summary>
        /// The given option contains a link that references a removed option (target).
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="link">The affected link.</param>
        public virtual void LinkInconsistent(string section, string option, Ini.Configuration.Base.ILink link)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' contains a link that references a removed option '{2}' or section '{3}'.", option, section, link.Target.Option, link.Target.Section));
        }

        /// <summary>
        /// The given option contains a value that is not allowed.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueNotAllowed(string section, string option, Ini.Configuration.Base.IValue value)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' contains a value that is not explicitly allowed ('{2}').", option, section, value.ToString()));
        }

        /// <summary>
        /// The given option contains a value that is out of range.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueOutOfRange(string section, string option, Ini.Configuration.Base.IValue value)
        {
            this.writer.WriteLine(string.Format("ERROR: option '{0}' in section '{1}' contains a value that is out of range ('{2}').", option, section, value.ToString()));
        }

        #endregion
	}
}
