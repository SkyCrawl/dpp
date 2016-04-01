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
    public class ConsoleSpecReaderBacklog : ISpecReaderBacklog
    {
        #region ISpecReaderBacklog Members

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="specOrigin">Config path.</param>
        public void NewSpec(string specOrigin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Information about a general parsing error occured.
        /// </summary>
        /// <param name="message"></param>
        public void ParsingError(string message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
