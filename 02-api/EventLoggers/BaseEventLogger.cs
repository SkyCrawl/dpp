using System.IO;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Base class for loggers that use <see cref="TextWriter"/>.
    /// </summary>
    public abstract class BaseEventLogger
    {
        #region Consts

        /// <summary>
        /// The string used to separate main sections of the log.
        /// </summary>
        protected const string LOG_SEPARATOR = "-----";

        #endregion

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
