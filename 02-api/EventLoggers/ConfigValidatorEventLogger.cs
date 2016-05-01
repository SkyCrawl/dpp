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
	}
}
