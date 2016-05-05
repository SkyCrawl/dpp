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
        /// The specification validator event logger.
        /// </summary>
        protected ISpecValidatorEventLogger specValidatorEventLogger;

        /// <summary>
        /// The configuration writer event logger.
        /// </summary>
        protected IConfigWriterEventLogger configWriterEventLogger;

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
            this.specValidatorEventLogger = new SchemaValidatorEventLogger(specValidatorOutput ?? Console.Out);
            this.configWriterEventLogger = new ConfigWriterEventLogger(configWriterOutput ?? Console.Out);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigWriter"/> class, with
        /// user-defined loggers.
        /// </summary>
        /// <param name="specValidatorEventLogger">Specification validation event logger.</param>
        /// <param name="configWriterEventLogger">Configuration writer event logger.</param>
        public ConfigWriter(ISpecValidatorEventLogger specValidatorEventLogger, IConfigWriterEventLogger configWriterEventLogger)
        {
            this.specValidatorEventLogger = specValidatorEventLogger ?? new SchemaValidatorEventLogger(Console.Out);
            this.configWriterEventLogger = configWriterEventLogger ?? new ConfigWriterEventLogger(Console.Out);
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
            if(options.ValidateConfig && !configuration.IsValid(options.ValidationMode, configWriterEventLogger, specValidatorEventLogger))
            {
                configWriterEventLogger.ConfigurationNotValid();
                throw new InvalidConfigException();
            }
            else
            {
                // and only then proceed with the writing; TODO: don't forget about the sorting order
                configuration.WriteTo(writer, options);
            }
        }

        #endregion

        #region Internal Methods

        internal static void WriteComment(TextWriter writer, string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                writer.WriteLine();
            }
            else
            {
                writer.Write("; ");
                writer.WriteLine(comment);
            }
        }

        #endregion
    }
}
