using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ini.Configuration;
using Ini.Specification;
using Ini.EventLoggers;
using Ini.Util;

namespace Ini
{
    /// <summary>
    /// The class used to read configuration from various sources. While consumers
    /// can specify arbitrary output for the event loggers (see constructors), they
    /// are not public so as to avoid unnecessary issues when manipulating with them
    /// while a task is running.
    /// </summary>
    public class ConfigReader
    {
        #region Fields

        /// <summary>
        /// The parser to use.
        /// </summary>
        protected IConfigParser parser;

        /// <summary>
        /// The configuration reader event logger.
        /// </summary>
        protected IConfigReaderEventLogger configEventLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigReader"/> class, with
        /// user-defined parser and logger output.
        /// </summary>
        /// <param name="parser">The parser to use.</param>
        /// <param name="specEventOutput">Specification validator event logger output.</param>
        /// <param name="configEventOutput">Configuration reader event logger output.</param>
        public ConfigReader(IConfigParser parser = null, TextWriter specEventOutput = null, TextWriter configEventOutput = null)
        {
            this.parser = parser ?? new ConfigParser();
            this.configEventLogger = new ConfigReaderEventLogger(configEventOutput, specEventOutput);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigReader"/> class, with
        /// user-defined parser and loggers.
        /// </summary>
        /// <param name="parser">The parser to use.</param>
        /// <param name="configEventLogger">Configuration reader event logger.</param>
        public ConfigReader(IConfigParser parser, IConfigReaderEventLogger configEventLogger)
        {
            this.parser = parser ?? new ConfigParser();
            this.configEventLogger = configEventLogger ?? new ConfigReaderEventLogger(Console.Out);
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
                Config result = LoadFromText(configPath, new StreamReader(fileStream, encoding), spec, mode);
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
        /// <param name="mode">The config validation mode.</param>
        /// <param name="spec">The configuration schema.</param>
        /// <param name="encoding">The file encoding.</param>
        public bool TryLoadFromFile(string configPath, out Config configuration, ConfigSpec spec, ConfigValidationMode mode = ConfigValidationMode.Strict, Encoding encoding = null)
        {
            if(encoding == null)
            {
                encoding = Encoding.Default;
            }
            using (var fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read))
            {
                bool result = TryLoadFromText(configPath, new StreamReader(fileStream, encoding), out configuration, spec, mode);
                configuration.Origin = configPath;
                return result;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>The config read and parsed from the given reader.</returns>
        /// <param name="origin">The configuration's origin. Will be forwarded into the logger.</param>
        /// <param name="reader">The given reader.</param>
        /// <param name="spec">The schema.</param>
        /// <param name="mode">The validation mode.</param>
        /// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
        /// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
        /// <exception cref="Ini.Exceptions.MalformedConfigException">If the configuration's format is malformed.</exception>
        public Config LoadFromText(string origin, TextReader reader, ConfigSpec spec, ConfigValidationMode mode = ConfigValidationMode.Strict)
        {
            parser.Prepare(spec, configEventLogger);
            return parser.Parse(reader, mode);
        }

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="origin">The configuration's origin. Will be forwarded into the logger.</param>
        /// <param name="reader">The given reader.</param>
        /// <param name="configuration">The config read and parsed from the given reader.</param>
        /// <param name="spec">The schema.</param>
        /// <param name="mode">The validation mode.</param>
        public bool TryLoadFromText(string origin, TextReader reader, out Config configuration, ConfigSpec spec, ConfigValidationMode mode = ConfigValidationMode.Strict)
        {
            parser.Prepare(spec, configEventLogger);
            try
            {
                configuration = parser.Parse(reader, mode);
                return true;
            }
            catch (Exception)
            {
                configuration = null;
                return false;
            }
        }

        #endregion
    }
}
