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
        public void NewSpecification(string specOrigin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A general parsing/format error has occurred.
        /// </summary>
        /// <param name="message"></param>
        public void SpecificationMalformed(string message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
