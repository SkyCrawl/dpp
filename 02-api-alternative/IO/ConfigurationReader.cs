using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Backlogs;
using IniConfiguration.Definitions;
using IniConfiguration.Interfaces;
using IniConfiguration.Schema;

namespace IniConfiguration
{
    /// <summary>
    /// The class used to read configuration from various sources.
    /// </summary>
    public class ConfigurationReader
    {
        #region Fields

        /// <summary>
        /// A user-specified or default backlog for handling errors and parsing messages.
        /// </summary>
        private IParsingBacklog backlog;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationReader"/> class.
        /// </summary>
        /// <param name="backlog">The backlog.</param>
        public ConfigurationReader(IParsingBacklog backlog = null)
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
        public Configuration LoadFromFile(string filePath, ValidationMode mode = ValidationMode.Strict, ConfigurationDefinition schema = null, Encoding encoding = null)
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
        public bool TryLoadFromFile(string filePath, out Configuration configuration, ValidationMode mode = ValidationMode.Strict, ConfigurationDefinition schema = null, Encoding encoding = null)
        {
            try
            {
                configuration = LoadFromFile(filePath, mode, schema, encoding);
                return true;
            }
            catch (Exception)
            {
                configuration = null;
                return false;
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
        public Configuration LoadFromText(TextReader reader, ValidationMode mode = ValidationMode.Strict, ConfigurationDefinition schema = null)
        {
            ConfigurationParser parser = new ConfigurationParser(schema);
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
        public bool TryLoadFromText(TextReader reader, out Configuration configuration, ValidationMode mode = ValidationMode.Strict, ConfigurationDefinition schema = null)
        {
            try
            {
                configuration = LoadFromText(reader, mode, schema);
                return true;
            }
            catch(Exception)
            {
                configuration = null;
                return false;
            }
        }

        #endregion
    }
}
