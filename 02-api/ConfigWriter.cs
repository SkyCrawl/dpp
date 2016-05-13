using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ini.Configuration;
using Ini.EventLoggers;
using Ini.Util;
using Ini.Exceptions;

namespace Ini
{
    /// <summary>
    /// The class used to write a configuration into stream.
    /// </summary>
    public class ConfigWriter
    {
        #region Properties

        /// <summary>
        /// The configuration writer event logger.
        /// </summary>
        protected IConfigWriterEventLogger eventLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigWriter"/> class, with
        /// user-defined logger output.
        /// </summary>
        /// <param name="specValidatorOutput">Specification validation event logger output.</param>
        /// <param name="configWriterOutput">Configuration writer event logger output.</param>
        public ConfigWriter(TextWriter specValidatorOutput = null, TextWriter configWriterOutput = null)
        {
            this.eventLogger = new ConfigWriterEventLogger(configWriterOutput ?? Console.Out, specValidatorOutput);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigWriter"/> class, with
        /// user-defined loggers.
        /// </summary>
        /// <param name="eventLogger">Configuration writer event logger.</param>
        public ConfigWriter(IConfigWriterEventLogger eventLogger)
        {
            this.eventLogger = eventLogger ?? new ConfigWriterEventLogger(Console.Out);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes the configuration into a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <param name="encoding"></param>
        /// <exception cref="UndefinedSpecException">If the configuration's specification is undefined.</exception>
        /// <exception cref="InvalidSpecException">If the configuration's specification is invalid.</exception>
        /// <exception cref="InvalidConfigException">If the configuration is invalid.</exception>
        public void WriteToFile(string fileName, Config configuration, ConfigWriterOptions options = null, Encoding encoding = null)
        {
            if(encoding == null)
            {
                encoding = Encoding.Default;
            }
            using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                var writer = new StreamWriter(stream, encoding);
                WriteToText(writer, configuration, options);
            }
        }

        /// <summary>
        /// Writes the configuration into a text writer.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <exception cref="UndefinedSpecException">If the configuration's specification is undefined.</exception>
        /// <exception cref="InvalidSpecException">If the configuration's specification is invalid.</exception>
        /// <exception cref="InvalidConfigException">If the configuration is invalid.</exception>
        public void WriteToText(TextWriter writer, Config configuration, ConfigWriterOptions options = null)
        {
            // first check validity of both specification and configuration, if defined and required
            options = options ?? ConfigWriterOptions.Default;
            if(options.Validate && !configuration.IsValid(options.ValidationMode, eventLogger.ConfigValidationLogger))
            {
                eventLogger.InvalidConfiguration();
                throw new InvalidConfigException();
            }

            // and only then proceed with the writing
            configuration.WriteTo(writer, options, eventLogger);
        }

        #endregion
    }
}
