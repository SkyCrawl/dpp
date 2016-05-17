using System;
using System.IO;
using Ini.Properties;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="IConfigValidatorEventLogger"/> that writes a text writer.
    /// </summary>
    public class ConfigValidatorEventLogger : BaseEventLogger, IConfigValidatorEventLogger
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigValidatorEventLogger"/> class.
        /// </summary>
        /// <param name="writer">Output stream for configuration validation events.</param>
        /// <param name="specValidationWriter">Output stream for specification validation events.</param>
        public ConfigValidatorEventLogger(TextWriter writer, TextWriter specValidationWriter = null)
            : base(writer)
        {
            SpecValidatiorLogger = CreateSpecValidatiorLogger(specValidationWriter ?? writer);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates the inner instance of the <see cref="ISpecValidatorEventLogger"/>.
        /// </summary>
        /// <param name="specValidationWriter">Output stream for specification validation events.</param>
        /// <returns></returns>
        protected ISpecValidatorEventLogger CreateSpecValidatiorLogger(TextWriter specValidationWriter)
        {
            var result = new SpecValidatorEventLogger(specValidationWriter);
            return result;
        }

        #endregion

        #region IConfigValidatorEventLogger Members

        /// <summary>
        /// The logger used for spec validation.
        /// </summary>
        public ISpecValidatorEventLogger SpecValidatiorLogger { get; private set; }

        /// <summary>
        /// Configuration can not be validated without a specification.
        /// </summary>
        public virtual void NoSpecification()
        {
            Writer.WriteLine(Resources.ConfigValidationNoSpecification);
        }

        /// <summary>
        /// Configuration can not be validated without a valid specification.
        /// </summary>
        public virtual void InvalidSpecification()
        {
            Writer.WriteLine(Resources.ConfigValidationInvalidSpecification);
        }

        /// <summary>
        /// Specification for the given section was not found when validating configuration.
        /// </summary>
        /// <param name="identifier">The section's identifier.</param>
        public virtual void NoSectionSpecification(string identifier)
        {
            Writer.WriteLine(Resources.ConfigValidationNoSectionSpecification, identifier);
        }

        /// <summary>
        /// Strict validation mode was applied and the configuration didn't contain the given mandatory section.
        /// </summary>
        /// <param name="identifier">The missing section's identifier.</param>
        public void MissingMandatorySection(string identifier)
        {
            Writer.WriteLine(Resources.ConfigValidationMissingMandatorySection, identifier);
        }

        /// <summary>
        /// Specification for the given option was not found when validating configuration.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void NoOptionSpecification(string section, string option)
        {
            Writer.WriteLine(Resources.ConfigValidationNoOptionSpecification, option, section);
        }

        /// <summary>
        /// Strict validation mode was applied and the configuration didn't contain the given mandatory option.
        /// </summary>
        /// <param name="section">The missing option's section identifier.</param>
        /// <param name="option">The missing option's identifier.</param>
        public void MissingMandatoryOption(string section, string option)
        {
            Writer.WriteLine(Resources.ConfigValidationMissingMandatoryOption, option, section);
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
            Writer.WriteLine(Resources.ConfigValidationValueTypeMismatch, option, section, typeOption.FullName, typeSpec.FullName);
        }

        /// <summary>
        /// The given option is mandatory but it doesn't contain a value.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void NoValue(string section, string option)
        {
            Writer.WriteLine(Resources.ConfigValidationNoValue, option, section);
        }

        /// <summary>
        /// The given option is declared as single-value but it contains multiple values.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        public virtual void TooManyValues(string section, string option)
        {
            Writer.WriteLine(Resources.ConfigValidationTooManyValues, option, section);
        }

        /// <summary>
        /// The given option contains a link that references a removed option (target).
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="link">The affected link.</param>
        public virtual void LinkInconsistent(string section, string option, Ini.Configuration.Base.ILink link)
        {
            Writer.WriteLine(Resources.ConfigValidationLinkInconsistent, option, section, link.Target.Option, link.Target.Section);
        }

        /// <summary>
        /// The given option contains a value that is not allowed.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueNotAllowed(string section, string option, Ini.Configuration.Base.IValue value)
        {
            Writer.WriteLine(Resources.ConfigValidationValueNotAllowed, option, section, value.ToString());
        }

        /// <summary>
        /// The given option contains a value that is out of range.
        /// </summary>
        /// <param name="section">The containing section's identifier.</param>
        /// <param name="option">The involved option's identifier.</param>
        /// <param name="value">The affected value.</param>
        public virtual void ValueOutOfRange(string section, string option, Ini.Configuration.Base.IValue value)
        {
            Writer.WriteLine(Resources.ConfigValidationValueOutOfRange, option, section, value.ToString());
        }

        #endregion
    }
}
