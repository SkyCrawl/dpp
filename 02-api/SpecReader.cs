using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini.Backlogs;
using Ini.Specification;

namespace Ini
{
    /// <summary>
    /// TODO
    /// </summary>
    public class SpecReader
    {
        #region Fields

        /// <summary>
        /// A user-specified or default backlog for handling errors and parsing messages.
        /// </summary>
        private ISchemaReaderBacklog backlog;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecReader"/> class.
        /// </summary>
        /// <param name="backlog">The backlog.</param>
        public SpecReader(ISchemaReaderBacklog backlog = null)
        {
            this.backlog = backlog ?? new ConsoleSchemaReaderBacklog();
			this.backlog.GetHashCode(); // TODO: remove! (this only gets rid of an annoying warning)
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
