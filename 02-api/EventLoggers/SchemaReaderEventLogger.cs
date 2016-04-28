using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="ISchemaReaderEventLogger"/> that writes a text writer.
    /// </summary>
    public class SchemaReaderEventLogger : SchemaValidatorEventLogger, ISchemaReaderEventLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaReaderEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public SchemaReaderEventLogger(TextWriter writer) : base(writer)
        {
        }

        #region ISpecReaderBacklog Members

        /// <summary>
        /// The associated reader will now parse a new schema. Consumers will
        /// probably want to distinguish the previous output from the new.
        /// </summary>
        /// <param name="specOrigin">Spec origin.</param>
        public void NewSpec(string specOrigin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A general parsing/format error has occurred.
        /// </summary>
        /// <param name="message"></param>
        public void SpecMalformed(string message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
