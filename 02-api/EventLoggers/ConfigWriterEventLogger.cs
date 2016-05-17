using System;
using System.Collections.Generic;
using System.IO;
using Ini.Configuration.Base;
using Ini.Properties;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="IConfigWriterEventLogger"/> forwarding output to the inherited <see cref="BaseEventLogger"/>.
    /// </summary>
    public class ConfigWriterEventLogger : BaseEventLogger, IConfigWriterEventLogger
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigWriterEventLogger"/> class.
        /// </summary>
        /// <param name="writer">Output stream for writing events.</param>
        /// <param name="configValidationWriter">Output stream for configuration validation events.</param>
        /// <param name="specValidationWriter">Output stream for specification validation events.</param>
        public ConfigWriterEventLogger(TextWriter writer, TextWriter configValidationWriter = null, TextWriter specValidationWriter = null)
            : base(writer)
        {
            ConfigValidatiorLogger = CreateValidatiorLogger(configValidationWriter ?? writer, specValidationWriter);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates the inner instance of the <see cref="IConfigValidatorEventLogger"/>.
        /// </summary>
        /// <param name="configValidationWriter">Output stream for configuration validation events.</param>
        /// <param name="specValidationWriter">Output stream for specification validation events.</param>
        /// <returns></returns>
        protected virtual IConfigValidatorEventLogger CreateValidatiorLogger(TextWriter configValidationWriter, TextWriter specValidationWriter)
        {
            var result = new ConfigValidatorEventLogger(configValidationWriter, specValidationWriter);
            return result;
        }

        #endregion

        #region IConfigWriterEventLogger Members

        /// <summary>
        /// Logger for configuration validation.
        /// </summary>
        /// <value>The config validation logger.</value>
        public IConfigValidatorEventLogger ConfigValidatiorLogger { get; private set; }

        /// <summary>
        /// The task's options instructed to use a specification for writing, but the configuration didn't have an associated specification.
        /// </summary>
        public void NoSpecification()
        {
            Writer.WriteLine(Resources.WriterNoSpecification);
        }

        /// <summary>
        /// The task's options instructed to validate the configuration before writing, and it was found to be invalid.
        /// </summary>
        public void InvalidConfiguration()
        {
            Writer.WriteLine(Resources.WriterInvalidConfiguration);
        }

        #endregion
    }
}
