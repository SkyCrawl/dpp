using System;
using System.Collections.Generic;

namespace Ini.Backlogs
{
    /// <summary>
    /// An implementation of writing backlog that writes into console.
    /// </summary>
    public class ConsoleWritingBacklog : IWritingBacklog
    {
        #region IWritingBacklog Members

        /// <summary>
        /// The configuration if not valid.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ConfigurationNotValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
