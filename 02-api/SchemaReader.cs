using System;
using System.IO;
using System.Text;
using Ini.EventLogs;
using Ini.Specification;

namespace Ini
{
    /// <summary>
    /// TODO
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
        /// Initializes a new instance of the <see cref="SchemaReader"/> class.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public SchemaReader(ISchemaReaderEventLog eventLog = null)
        {
            this.eventLog = eventLog ?? new ConsoleSchemaReaderEventLog();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an instance of <see cref="ConfigSpec"/> from the given path and encoding.
        /// </summary>
        /// <returns>The config read and parsed from the given path and encoding.</returns>
        /// <param name="filePath">The given path.</param>
        /// <param name="encoding">The given encoding.</param>
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
        /// Creates an instance of <see cref="ConfigSpec"/> from the given path and encoding.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="filePath">The given path.</param>
        /// <param name="spec">The config read and parsed from the given reader.</param>
        /// <param name="encoding">The given encoding.</param>
        /// <exception cref="Ini.Exceptions.MalformedSchemaException">If the schema is malformed.</exception>
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
        /// Creates an instance of <see cref="ConfigSpec"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>The config read and parsed from the given reader.</returns>
        /// <param name="reader">The given reader.</param>
        /// <exception cref="Ini.Exceptions.MalformedSchemaException">If the schema is malformed.</exception>
        public ConfigSpec LoadFromText(TextReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an instance of <see cref="ConfigSpec"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="reader">The given reader.</param>
        /// <param name="spec">The config read and parsed from the given reader.</param>
        /// <exception cref="Ini.Exceptions.MalformedSchemaException">If the schema is malformed.</exception>
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
