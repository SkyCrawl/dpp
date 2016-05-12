﻿using System;
using System.Collections.Generic;
using System.IO;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="IConfigWriterEventLogger"/> that writes a text writer.
    /// </summary>
    public class ConfigWriterEventLogger : IConfigWriterEventLogger
    {
        #region Fields

        TextWriter writer;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigWriterEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public ConfigWriterEventLogger(TextWriter writer)
        {
            this.writer = writer;
            ValidationLogger = new ConfigValidatorEventLogger(writer);
        }

        #endregion

        #region IConfigValidatorEventLogger Members

        /// <summary>
        /// Logger for configuration validation.
        /// </summary>
        public IConfigValidatorEventLogger ValidationLogger { get; private set; }

        /// <summary>
        /// Configuration validation was called before writing and the configuration is not valid.
        /// </summary>
        public void IsNotValid()
        {
            this.writer.WriteLine("ERROR: configuration can not be written because it is not valid.");
        }

        /// <summary>
        /// The specification must be present for selected writed options.
        /// </summary>
        public void NoSpecification()
        {
            this.writer.WriteLine("ERROR: configuration can not be written because the specification must be present for selected writer options.");
        }

        #endregion
    }
}
