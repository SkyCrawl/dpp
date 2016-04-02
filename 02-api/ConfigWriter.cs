using System;
using System.Collections.Generic;
using System.IO;
using Ini.Configuration;
using System.Text;
using Ini.Backlogs;
using Ini.Util;

namespace Ini
{
    /// <summary>
    /// The class used to write a configuration into stream.
    /// </summary>
    public class ConfigWriter
    {
        #region Fields

		private IWritingBacklog backlog;

        #endregion

        #region Constructor

        /// <summary>
		/// Initializes a new instance of the <see cref="ConfigWriter"/> class with an optional backlog.
        /// </summary>
		/// <param name="backlog">The backlog.</param>
        public ConfigWriter(IWritingBacklog backlog = null)
        {
            this.backlog = backlog ?? new ConsoleWritingBacklog();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes the configuration into a file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <param name="encoding"></param>
		public void WriteToFile(string fileName, Config configuration, WriterOptions options = null, Encoding encoding = null)
        {
			if(encoding == null)
			{
				encoding = Encoding.Default;
			}

            using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                var writer = new StreamWriter(stream, encoding);
                WriteToText(writer, configuration, options);
            }
        }

		/// <summary>
		/// Writes the configuration into a text writer.
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="configuration"></param>
		/// <param name="options"></param>
		public void WriteToText(TextWriter writer, Config configuration, WriterOptions options = null)
		{
			backlog.ConfigurationNotValid();
			throw new NotImplementedException();
		}

        #endregion
    }
}
