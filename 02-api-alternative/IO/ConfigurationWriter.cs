using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Backlogs;
using IniConfiguration.Definitions;
using IniConfiguration.Interfaces;

namespace IniConfiguration
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
        /// Initializes a new instance of the <see cref="ConfigurationWriter"/> class with an optional backlog.
        /// </summary>
        /// <param name="writingBacklog">The backlog.</param>
        public ConfigurationWriter(IWritingBacklog writingBacklog = null)
        {
            this.writingBacklog = writingBacklog ?? new ConsoleWritingBacklog();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes the configuration into a text writer.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        public void WriteToText(TextWriter writer, Configuration configuration, WritingOptions options = null)
        {
        }

        /// <summary>
        /// Writes the configuration into a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <param name="encoding"></param>
        public void WriteToFile(string fileName, Configuration configuration, WritingOptions options = null, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                var writer = new StreamWriter(stream, encoding);
                WriteToText(writer, configuration, options);
            }
        }

        #endregion
    }
}
