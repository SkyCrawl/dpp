using System;
using System.Collections.Generic;
using System.IO;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="IConfigWriterEventLogger"/> that writes a text writer.
    /// </summary>
    public class ConfigWriterEventLogger : ConfigValidatorEventLogger, IConfigWriterEventLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigWriterEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public ConfigWriterEventLogger(TextWriter writer) : base(writer)
        {
            this.writer = writer;
        }
    }
}
