using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ini.Configuration;
using Ini.EventLogs;
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
        /// The config writer event log.
        /// </summary>
        protected IConfigWriterEventLog configWriterEventLog;

        /// <summary>
        /// The spec validator event log.
        /// </summary>
        protected ISpecValidatorEventLog specValidatorEventLog;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigWriter"/> class
        /// with an option to supply user defined validation and writer log.
        /// </summary>
        /// <param name="specValidatorEventLog">Specification validator event log.</param>
        /// <param name="configWriterEventLog">Configuration writer event log.</param>
        public ConfigWriter(ISpecValidatorEventLog specValidatorEventLog = null, IConfigWriterEventLog configWriterEventLog = null)
        {
            this.configWriterEventLog = configWriterEventLog ?? new ConsoleConfigWriterEventLog();
            this.specValidatorEventLog = specValidatorEventLog ?? new ConsoleSchemaValidatorEventLog();
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
            if(options.ValidateConfig && !configuration.IsValid(options.ValidationMode, configWriterEventLog, specValidatorEventLog))
            {
                configWriterEventLog.ConfigNotValid();
                throw new InvalidConfigException();
            }
            else
            {
                // and only then proceed with the writing; TODO: don't forget about the sorting order
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
