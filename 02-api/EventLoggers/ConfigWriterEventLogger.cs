using System;
using System.Collections.Generic;
using System.IO;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="IConfigWriterEventLogger"/> forwarding output to the inherited <see cref="BaseEventLogger"/>.
    /// </summary>
    public class ConfigWriterEventLogger : BaseEventLogger, IConfigWriterEventLogger
    {
        #region Properties

        /// <summary>
        /// Logger for specification validation.
        /// </summary>
        /// <value>The spec validation logger.</value>
        public ISpecValidatorEventLogger SpecValidationLogger { get; private set; }

        /// <summary>
        /// Logger for configuration validation.
        /// </summary>
        /// <value>The config validation logger.</value>
        public IConfigValidatorEventLogger ConfigValidationLogger { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigWriterEventLogger"/> class.
        /// </summary>
        /// <param name="writer">Output stream for writing events.</param>
        /// <param name="specValidationWriter">Output stream for specification validation events.</param>
        /// <param name="configValidationWriter">Output stream for configuration validation events.</param>
        public ConfigWriterEventLogger(TextWriter writer, TextWriter specValidationWriter = null, TextWriter configValidationWriter = null)
            : base(writer)
        {
            SpecValidationLogger = new SpecValidatorEventLogger(specValidationWriter ?? writer);
            ConfigValidationLogger = new ConfigValidatorEventLogger(configValidationWriter ?? writer, specValidationWriter ?? writer);
        }

        #endregion

        #region IConfigWriterEventLogger Members

        /// <summary>
        /// The task's options instructed to use a specification for writing, but the configuration didn't have an associated specification.
        /// </summary>
        public void NoSpecification()
        {
            Writer.WriteLine("ERROR: no specification to use. Stopping...");
            Writer.WriteLine("\tHint: either associate the configuration with a specification or try again with different options.");
        }

        /// <summary>
        /// The task's options instructed to validate the configuration before writing, and it was found to be invalid.
        /// </summary>
        public void InvalidConfiguration()
        {
            Writer.WriteLine("ERROR: invalid configuration. Stopping...");
            Writer.WriteLine("\tHint: either correct the configuration or try again with validation disabled.");
        }

        #endregion
    }
}
