using System;
using System.Collections.Generic;

namespace Ini.Backlogs
{
    /// <summary>
    /// The implementation of a parsing backlog that logs into console.
    /// </summary>
    public class ConsoleParsingBacklog : IParsingBacklog
    {
        #region IParsingBacklog Members

		/// <summary>
		/// Information about parsing a new configuration. Users will probably
		/// want to distinguish the previous output from the current.
		/// </summary>
		/// <param name="configPath">Config path.</param>
		/// <param name="schemaPath">Schema path.</param>
		/// <param name="mode">Mode.</param>
		public void NewConfig(string configPath, string schemaPath = null, ValidationMode mode = ValidationMode.Strict)
		{
			throw new NotImplementedException();
		}

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
