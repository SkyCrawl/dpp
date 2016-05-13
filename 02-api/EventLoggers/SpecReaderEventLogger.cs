using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            Writer.WriteLine(new String('-', 5));
            Writer.WriteLine("...Commencing new specification parsing task.");
            if(specOrigin != null)
            {
                Writer.WriteLine("\tOrigin: " + specOrigin);
            }
        }

        /// <summary>
        /// A general parsing/format error has occurred.
        /// </summary>
        /// <param name="e">The exception that triggered the event.</param>
        public virtual void MalformedSpecification(Exception e)
        {
            Writer.WriteLine("ERROR: couldn't read the specification because of the reason that follows.");
            Writer.WriteLine("\t" + e.StackTrace);
        }

        #endregion
    }
}
