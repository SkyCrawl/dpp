using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="ISpecReaderEventLogger"/> that writes a text writer.
    /// </summary>
    public class SpecReaderEventLogger : SpecValidatorEventLogger, ISpecReaderEventLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecReaderEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public SpecReaderEventLogger(TextWriter writer) : base(writer)
        {
        }

        #region ISpecReaderBacklog Members

        /// <summary>
        /// The associated reader will now parse a new schema. Consumers will
        /// probably want to distinguish the previous output from the new.
        /// </summary>
        /// <param name="specOrigin">Spec origin.</param>
        public virtual void NewSpecification(string specOrigin)
        {
            this.writer.WriteLine(new String('-', 5));
            this.writer.WriteLine("...Commencing new specification parsing task.");
            if(specOrigin != null)
            {
                this.writer.WriteLine("\tOrigin: " + specOrigin);
            }
        }

        /// <summary>
        /// A general parsing/format error has occurred.
        /// </summary>
        /// <param name="e">The exception that triggered the event.</param>
        public virtual void SpecificationMalformed(Exception e)
        {
            this.writer.WriteLine("ERROR: couldn't read the specification because of the reason that follows.");
            this.writer.WriteLine("\t" + e.StackTrace);
        }

        #endregion
    }
}
