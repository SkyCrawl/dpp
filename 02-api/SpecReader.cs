using System;
using System.IO;
using System.Text;
using Ini.EventLoggers;
using Ini.Exceptions;
using Ini.Specification;
using YamlDotNet.Serialization;

namespace Ini
{
    /// <summary>
    /// The class used to parse configuration schema.
    /// </summary>
    public class SpecReader
    {
        #region Fields

        /// <summary>
        /// The specification reader event logger.
        /// </summary>
        protected ISpecReaderEventLogger eventLogger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.SpecReader"/> class, with
        /// user-defined logger output.
        /// </summary>
        /// <param name="specReaderOutput">Specification reader event logger output.</param>
        public SpecReader(TextWriter specReaderOutput = null)
        {
            this.eventLogger = new SpecReaderEventLogger(specReaderOutput ?? Console.Out);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.SpecReader"/> class, with
        /// user-defined logger.
        /// </summary>
        /// <param name="eventLogger">Specification reader event logger.</param>
        public SpecReader(ISpecReaderEventLogger eventLogger)
        {
            this.eventLogger = eventLogger ?? new SpecReaderEventLogger(Console.Out);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an instance of <see cref="ConfigSpec"/> from a YAML file.
        /// </summary>
        /// <returns>The configuration specification read and parsed from the given path and encoding.</returns>
        /// <param name="filePath">The path to a file in YAML format.</param>
        /// <param name="encoding">The file encoding.</param>
        /// <exception cref="MalformedSpecException">If the schema is malformed.</exception>
        public ConfigSpec LoadFromFile(string filePath, Encoding encoding = null)
        {
            if(encoding == null)
            {
                encoding = Encoding.Default;
            }
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return LoadFromText(filePath, new StreamReader(fileStream, encoding));
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
        /// <param name="origin">The specification's origin. Will be forwarded into the logger.</param>
        /// <param name="reader">The object to read YAML specification from.</param>
        /// <exception cref="MalformedSpecException">If the schema is malformed.</exception>
        public ConfigSpec LoadFromText(string origin, TextReader reader)
        {
            // prepare the YAML deserializer that can resolve custom types
            var deserializer = new Deserializer();
            deserializer.TypeResolvers.Insert(0, new SpecTypeResolver());

            // and try to deserialize
            try
            {
                eventLogger.NewSpecification(origin);
                return deserializer.Deserialize<ConfigSpec>(reader);
            }
            catch (Exception e)
            {
                throw new MalformedSpecException("Unable to deserialize specification.", e);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="ConfigSpec"/> from the given reader. Can be used to load a configuration
        /// specification from a file or memory.
        /// </summary>
        /// <returns>True if the configuration is parsed successfully.</returns>
        /// <param name="origin">The specification's origin. Will be forwarded into the logger.</param>
        /// <param name="reader">The reader with a specification in YAML format.</param>
        /// <param name="spec">The configuration specification read and parsed from the given reader.</param>
        public bool TryLoadFromText(string origin, TextReader reader, out ConfigSpec spec)
        {
            try
            {
                spec = LoadFromText(origin, reader);
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
