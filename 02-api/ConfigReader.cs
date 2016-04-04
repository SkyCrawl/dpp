using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ini.Configuration;
using Ini.Specification;
using Ini.Backlogs;
using Ini.Util;
using Ini.Validation;

namespace Ini
{
    /// <summary>
    /// The class used to read configuration from various sources.
    /// </summary>
    public class ConfigReader
    {
        #region Fields

        /// <summary>
        /// TODO
        /// </summary>
		protected IConfigReaderBacklog configBacklog;

		/// <summary>
		/// TODO
		/// </summary>
		protected ISpecValidatorBacklog specBacklog;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigReader"/> class.
        /// </summary>
        /// <param name="configBacklog">Config backlog.</param>
        /// <param name="specBacklog">Spec backlog.</param>
		public ConfigReader(IConfigReaderBacklog configBacklog = null, ISpecValidatorBacklog specBacklog = null)
        {
            this.configBacklog = configBacklog ?? new ConsoleConfigReaderBacklog();
			this.specBacklog = specBacklog ?? new ConsoleSchemaValidatorBacklog();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given path and encoding.
        /// </summary>
        /// <returns>The config read and parsed from the given path and encoding.</returns>
		/// <param name="configPath">The given path.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="spec">The schema.</param>
        /// <param name="encoding">The given encoding.</param>
		/// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
		/// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
		/// <exception cref="Ini.Exceptions.MalformedConfigException">If the configuration's format is malformed.</exception>
		public Config LoadFromFile(string configPath, ConfigSpec spec, ConfigValidationMode mode = ConfigValidationMode.Strict, Encoding encoding = null)
        {
			if(encoding == null)
			{
				encoding = Encoding.Default;
			}
			using (var fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read))
            {
				Config result = LoadFromText(new StreamReader(fileStream, encoding), spec, mode);
				result.Origin = configPath;
				return result;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given path and encoding.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
		/// <param name="configPath">The given path.</param>
        /// <param name="configuration">The config read and parsed from the given reader.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="spec">The schema.</param>
        /// <param name="encoding">The given encoding.</param>
		/// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
		/// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
		/// <exception cref="Ini.Exceptions.MalformedConfigException">If the configuration's format is malformed.</exception>
		public bool TryLoadFromFile(string configPath, out Config configuration, ConfigSpec spec, ConfigValidationMode mode = ConfigValidationMode.Strict, Encoding encoding = null)
        {
			if(encoding == null)
			{
				encoding = Encoding.Default;
			}
			using (var fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read))
            {
				bool result = TryLoadFromText(new StreamReader(fileStream, encoding), out configuration, spec, mode);
				configuration.Origin = configPath;
				return result;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>The config read and parsed from the given reader.</returns>
        /// <param name="reader">The given reader.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="spec">The schema.</param>
		/// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
		/// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
		/// <exception cref="Ini.Exceptions.MalformedConfigException">If the configuration's format is malformed.</exception>
		public Config LoadFromText(TextReader reader, ConfigSpec spec, ConfigValidationMode mode = ConfigValidationMode.Strict)
        {
            ConfigParser parser = new ConfigParser(spec);
			return parser.Parse(reader, configBacklog, specBacklog, mode);
        }

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="reader">The given reader.</param>
        /// <param name="configuration">The config read and parsed from the given reader.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="spec">The schema.</param>
		/// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
		/// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
		/// <exception cref="Ini.Exceptions.MalformedConfigException">If the configuration's format is malformed.</exception>
		public bool TryLoadFromText(TextReader reader, out Config configuration, ConfigSpec spec, ConfigValidationMode mode = ConfigValidationMode.Strict)
        {
            ConfigParser parser = new ConfigParser(spec);
			return parser.TryParse(reader, out configuration, configBacklog, specBacklog, mode);
        }

        #endregion
    }
}
