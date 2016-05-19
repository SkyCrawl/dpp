using System.IO;
using Ini.Properties;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="ISpecReaderEventLogger"/> forwarding output to the inherited <see cref="BaseEventLogger"/>.
    /// </summary>
    public class SpecReaderEventLogger : BaseEventLogger, ISpecReaderEventLogger
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecReaderEventLogger"/> class.
        /// </summary>
        /// <param name="writer">Output stream for reading events.</param>
        public SpecReaderEventLogger(TextWriter writer)
            : base(writer) { }

        #endregion

        #region ISpecReaderBacklog Members

        /// <summary>
        /// The associated reader will now parse a new schema. Consumers will
        /// probably want to distinguish the previous output from the new.
        /// </summary>
        /// <param name="specOrigin">Spec origin.</param>
        public virtual void NewSpecification(string specOrigin)
        {
            Writer.WriteLine(LOG_SEPARATOR);
            Writer.WriteLine(Resources.SpecReaderNewSpecification);
            if(specOrigin != null)
            {
                Writer.WriteLine(Resources.ReaderOrigin, specOrigin);
            }
        }

        #endregion
    }
}
