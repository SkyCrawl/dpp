using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface defining specification reading events.
    /// </summary>
    public interface ISpecReaderEventLogger
    {
        /// <summary>
        /// A new specification reading task has commenced.
        /// </summary>
        /// <param name="schemaOrigin">Origin of the newly parsed specification.</param>
        void NewSpecification(string schemaOrigin);

        /// <summary>
        /// A general parsing/format error has occurred.
        /// </summary>
        /// <param name="message"></param>
        void SpecificationMalformed(string message);
    }
}
