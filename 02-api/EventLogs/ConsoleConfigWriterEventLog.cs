using System;
using System.Collections.Generic;

namespace Ini.EventLogs
{
    /// <summary>
    /// An implementation of <see cref="IConfigWriterEventLog"/> that writes into the console.
    /// </summary>
    public class ConsoleConfigWriterEventLog : ConsoleConfigValidatorEventLog, IConfigWriterEventLog
    {
        #region IConfigWriterBacklog implementation

        /// <summary>
        /// Specs the not valid.
        /// </summary>
        public void SpecNotValid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Configs the not valid.
        /// </summary>
        public void ConfigNotValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
