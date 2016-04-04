using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ini.Configuration;
using Ini.Backlogs;
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
		/// The config writer backlog.
		/// </summary>
		protected IConfigWriterBacklog configWriterBacklog;

		/// <summary>
		/// The spec validator backlog.
		/// </summary>
		protected ISpecValidatorBacklog specValidatorBacklog;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigWriter"/> class.
        /// </summary>
		/// <param name="specValidatorBacklog">Spec validator backlog.</param>
		/// <param name="configWriterBacklog">Config writer backlog.</param>
		public ConfigWriter(ISpecValidatorBacklog specValidatorBacklog = null, IConfigWriterBacklog configWriterBacklog = null)
        {
			this.configWriterBacklog = configWriterBacklog ?? new ConsoleConfigWriterBacklog();
			this.specValidatorBacklog = specValidatorBacklog ?? new ConsoleSchemaValidatorBacklog();
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
			options = options ?? ConfigWriterOptions.GetDefault();
			if(options.ValidateConfig && !configuration.IsValid(options.ValidationMode, configWriterBacklog, specValidatorBacklog))
			{
				configWriterBacklog.ConfigNotValid();
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
