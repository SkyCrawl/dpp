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

        #region IConfigWriterEventLogger implementation

        /// <summary>
        /// One of the links in the configuration was inconsistent with it's origin option.
        /// </summary>
        /// <param name="section">The link section</param>
        /// <param name="option">The link option</param>
        /// <param name="link">The link instance</param>
        public void LinkInconsistent(string section, string option, ILink link)
        {
            this.writer.WriteLine("The link pointing on section {0} and option {1} is inconsistent.", section, option);
        }

        #endregion
    }
}
