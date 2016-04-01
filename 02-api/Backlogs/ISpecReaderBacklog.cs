using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.Backlogs
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface ISpecReaderBacklog
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="specOrigin">Config path.</param>
        void NewSpec(string specOrigin);

        /// <summary>
        /// Information about a general parsing error occured.
        /// </summary>
        /// <param name="message"></param>
        void ParsingError(string message);
    }
}
