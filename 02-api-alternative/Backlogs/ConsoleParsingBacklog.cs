using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Interfaces;

namespace IniConfiguration.Backlogs
{
    /// <summary>
    /// The implementation of a parsing backlog that logs into console.
    /// </summary>
    public class ConsoleParsingBacklog : IParsingBacklog
    {
        #region IParsingBacklog Members

        /// <summary>
        /// A parsing error occured at the specified line.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="message"></param>
        public void ParsingError(int lineIndex, string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A duplicate section was found in configuration.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="sectionName"></param>
        public void DuplicateSection(int lineIndex, string sectionName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
