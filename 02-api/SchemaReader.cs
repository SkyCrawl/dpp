using System;
using System.IO;
using System.Text;
using Ini.EventLogs;
using Ini.Specification;

namespace Ini
{
    /// <summary>
    /// The class used to parse configuration schema.
    /// </summary>
    public class SchemaReader
    {
        #region Fields

        /// <summary>
        /// A user-specified or default event log for handling errors and parsing messages.
        /// </summary>
        protected ISchemaReaderEventLog eventLog;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaReader"/> class
        /// with an option to supply a user defined event log.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public SchemaReader(ISchemaReaderEventLog eventLog = null)
        {
            this.eventLog = eventLog ?? new ConsoleSchemaReaderEventLog();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an instance of <see cref="ConfigSpec"/> from a YAML file.
        /// </summary>
        /// <returns>The configuration specification read and parsed from the given path and encoding.</returns>
        /// <param name="filePath">The path to a file in YAML format.</param>
        /// <param name="encoding">The file encoding.</param>
        /// <exception cref="Ini.Exceptions.MalformedSchemaException">If the schema is malformed.</exception>
        public ConfigSpec LoadFromFile(string filePath, Encoding encoding = null)
        {
            if(encoding == null)
            {
                encoding = Encoding.Default;
            }
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return LoadFromText(new StreamReader(fileStream, encoding));
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="ConfigSpec"/> from a YAML file.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="filePath">The path to a file in YAML format.</param>
        /// <param name="spec">The config read and parsed from the given reader.</param>
        /// <param name="encoding">The given encoding.</param>
        public bool TryLoadFromFile(string filePath, out ConfigSpec spec, Encoding encoding = null)
        {
            try
            {
                spec = LoadFromFile(filePath, encoding);
                return true;
            }
            catch (Exception)
            {
                spec = null;
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="ConfigSpec"/> from the given reader. Can be used to load a configuration
        /// specification from a file or memory.
        /// </summary>
        /// <returns>The configuration specification read and parsed from the given reader.</returns>
        /// <param name="reader">The reader with a specification in YAML format.</param>
        /// <exception cref="Ini.Exceptions.MalformedSchemaException">If the schema is malformed.</exception>
        public ConfigSpec LoadFromText(TextReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an instance of <see cref="ConfigSpec"/> from the given reader. Can be used to load a configuration
        /// specification from a file or memory.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="reader">The reader with a specification in YAML format.</param>
        /// <param name="spec">The configuration specification read and parsed from the given reader.</param>
        public bool TryLoadFromText(TextReader reader, out ConfigSpec spec)
        {
            try
            {
                spec = LoadFromText(reader);
                return true;
            }
            catch(Exception)
            {
                spec = null;
                return false;
            }
        }
        
        #endregion
    }
}
