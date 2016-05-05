﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="IConfigWriterEventLogger"/> that writes a text writer.
    /// </summary>
    public class ConfigWriterEventLogger : ConfigValidatorEventLogger, IConfigWriterEventLogger
    {
        #region Properties

        /// <summary>
        /// The output stream to write event logs to.
        /// </summary>
        protected TextWriter writer;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigWriterEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public ConfigWriterEventLogger(TextWriter writer) : base(writer)
        {
            this.writer = writer;
        }

        #region IConfigWriterBacklog implementation

        /// <summary>
        /// Specs the not valid.
        /// </summary>
        public void SpecificationNotValid()
        {
            this.writer.WriteLine("The supplied specification is invalid!");
        }

        /// <summary>
        /// Configs the not valid.
        /// </summary>
        public void ConfigurationNotValid()
        {
            this.writer.WriteLine("The supplied configuration is invalid!");
        }

        #endregion
    }
}
