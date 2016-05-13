using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Base class for loggers that use <see cref="TextWriter"/>.
    /// </summary>
    public abstract class BaseEventLogger
    {
        #region Properties

        /// <summary>
        /// The log output.
        /// </summary>
        protected TextWriter Writer { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The log output.</param>
        public BaseEventLogger(TextWriter writer)
        {
            Writer = writer;
        }

        #endregion
    }
}
