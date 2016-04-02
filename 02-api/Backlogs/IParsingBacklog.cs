using System;
using System.Collections.Generic;
using Ini.Util;

namespace Ini.Backlogs
{
    /// <summary>
    /// The interface for backlog that provides information about parsing configuration.
    /// </summary>
    public interface IParsingBacklog
    {
		/// <summary>
		/// Information about parsing a new configuration. Users will probably
		/// want to distinguish the previous output from the current.
		/// </summary>
		/// <param name="configPath">Config path.</param>
		/// <param name="schemaPath">Schema path.</param>
		/// <param name="mode">Mode.</param>
		void NewConfig(string configPath, string schemaPath = null, ValidationMode mode = ValidationMode.Strict);

        /// <summary>
        /// Information about a general parsing error occured at the specified line.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="message"></param>
        void ParsingError(int lineIndex, string message);

        /// <summary>
        /// An error A duplicate section was found in configuration.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="sectionName"></param>
        void DuplicateSection(int lineIndex, string sectionName);

        /// <summary>
        /// An error A duplicate option was found in configuration.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <param name="optionName"></param>
        void DuplicateOption(int lineIndex, string optionName);
    }
}
