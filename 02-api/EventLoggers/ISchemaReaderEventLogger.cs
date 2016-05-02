using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface providing information about reading schemas.
    /// </summary>
    public interface ISchemaReaderEventLogger
    {
        /// <summary>
        /// The associated reader will now parse a new schema. Consumers will
        /// probably want to distinguish the previous output from the new.
        /// </summary>
        /// <param name="schemaOrigin">Origin of the newly parsed schema.</param>
        void NewSpecification(string schemaOrigin);

        /// <summary>
        /// A general parsing/format error has occurred.
        /// </summary>
        /// <param name="message"></param>
        void SpecificationMalformed(string message);
    }
}
