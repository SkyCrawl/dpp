using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;
using _02_api_alternative.Interfaces;

namespace _02_api_alternative
{
    /// <summary>
    /// The class used to write a configuration into stream.
    /// </summary>
    public class ConfigurationWriter
    {
        #region Fields

        IWritingBacklog writingBacklog;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationWriter"/> class.
        /// </summary>
        public ConfigurationWriter()
            : this(new ConsoleWritingBacklog())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationWriter"/> class with an optional configuration schema.
        /// </summary>
        /// <param name="schema">The configuration schema.</param>
        public ConfigurationWriter(IWritingBacklog writingBacklog)
        {
            this.writingBacklog = writingBacklog;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes the configuration into a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="configuration"></param>
        public void WriteToText(TextWriter writer, Configuration configuration)
        {
        }

        public void WriteToFile(string fileName, Configuration configuration)
        {
            WriteToFile(fileName, Encoding.Default, configuration);
        }

        public void WriteToFile(string fileName, Encoding encoding, Configuration configuration)
        {
            using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                var writer = new StreamWriter(stream, encoding);
                WriteToText(writer, configuration);
            }
        }

        #endregion
    }
}
