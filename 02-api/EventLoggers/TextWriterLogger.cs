using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Base class for loggers, that use textwritter.
    /// </summary>
    public abstract class TextWriterLogger
    {
        #region Properties

        /// <summary>
        /// The writer used for log output.
        /// </summary>
        protected TextWriter Writer { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterLogger"/> class.
        /// </summary>
        /// <param name="writer">The text writer for log output.</param>
        public TextWriterLogger(TextWriter writer)
        {
            Writer = writer;
        }

        #endregion
    }
}
