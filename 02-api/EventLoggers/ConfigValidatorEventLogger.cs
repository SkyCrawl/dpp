using System;
using System.IO;

namespace Ini.EventLoggers
{
	/// <summary>
    /// An implementation of <see cref="IConfigValidatorEventLogger"/> that writes a text writer.
	/// </summary>
	public class ConfigValidatorEventLogger : IConfigValidatorEventLogger
	{
        /// <summary>
        /// The output stream to write event logs to.
        /// </summary>
        protected TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigValidatorEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public ConfigValidatorEventLogger(TextWriter writer)
        {
            this.writer = writer;
        }

        #region IConfigValidatorEventLog Members

        /// <summary>
		/// A duplicate section has been found in the associated configuration.
		/// </summary>
		/// <param name="lineIndex">Line number of the duplicate.</param>
		/// <param name="sectionIdentifier">Containing section's identifier.</param>
		public void DuplicateSection(int lineIndex, string sectionIdentifier)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// A duplicate option within a section has been found in the associated configuration.
		/// </summary>
		/// <param name="lineIndex">Line number of the duplicate.</param>
		/// <param name="sectionIdentifier">The containing section's identifier.</param>
		/// <param name="optionIdentifier">The involved option's identifier.</param>
		public void DuplicateOption(int lineIndex, string sectionIdentifier, string optionIdentifier)
		{
			throw new NotImplementedException();
		}

        #endregion
	}
}
