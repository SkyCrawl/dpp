using System;

namespace Ini.EventLogs
{
	/// <summary>
	/// An implementation of <see cref="IConfigValidatorEventLog"/> that writes into the console.
	/// </summary>
	public class ConsoleConfigValidatorEventLog : IConfigValidatorEventLog
	{
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
	}
}
