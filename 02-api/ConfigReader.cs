using System;
using System.Collections.Generic;
using System.IO;
using Ini.Configuration;
using System.Text;
using Ini.Schema;
using Ini.Backlogs;
using Ini.Util;

namespace Ini
{
    /// <summary>
    /// The class used to read configuration from various sources.
    /// </summary>
    public class ConfigReader
    {
        #region Fields

        /// <summary>
        /// A user-specified or default backlog for handling errors and parsing messages.
        /// </summary>
        private IParsingBacklog backlog;

        #endregion

        #region Constructor

        /// <summary>
		/// Initializes a new instance of the <see cref="ConfigReader"/> class.
        /// </summary>
        /// <param name="backlog">The backlog.</param>
        public ConfigReader(IParsingBacklog backlog = null)
        {
            this.backlog = backlog ?? new ConsoleParsingBacklog();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given path and encoding.
        /// </summary>
        /// <returns>The config read and parsed from the given path and encoding.</returns>
        /// <param name="filePath">The given path.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="schema">The schema.</param>
        /// <param name="encoding">The given encoding.</param>
        public Config LoadFromFile(string filePath, ValidationMode mode = ValidationMode.Strict, ConfigSpec schema = null, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return LoadFromText(new StreamReader(fileStream, encoding), mode, schema);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given path and encoding.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="filePath">The given path.</param>
        /// <param name="configuration">The config read and parsed from the given reader.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="schema">The schema.</param>
        /// <param name="encoding">The given encoding.</param>
		public bool TryLoadFromFile(string filePath, out Config configuration, ValidationMode mode = ValidationMode.Strict, ConfigSpec schema = null, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return TryLoadFromText(new StreamReader(fileStream, encoding), out configuration, mode, schema);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>The config read and parsed from the given reader.</returns>
        /// <param name="reader">The given reader.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="schema">The schema.</param>
		public Config LoadFromText(TextReader reader, ValidationMode mode = ValidationMode.Strict, ConfigSpec schema = null)
        {
            ConfigParser parser = new ConfigParser(schema);
            return parser.Parse(reader, backlog, mode);
        }

        /// <summary>
        /// Creates an instance of <see cref="Configuration"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="reader">The given reader.</param>
        /// <param name="configuration">The config read and parsed from the given reader.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="schema">The schema.</param>
		public bool TryLoadFromText(TextReader reader, out Config configuration, ValidationMode mode = ValidationMode.Strict, ConfigSpec schema = null)
        {
            ConfigParser parser = new ConfigParser(schema);
            return parser.TryParse(reader, out configuration, backlog, mode);
        }

        #endregion
    }
}
