using System;

namespace Ini
{
	/// <summary>
	/// Interface providing information about validating configurations.
	/// </summary>
	public interface IConfigValidatorEventLog
	{
		/// <summary>
		/// A duplicate section has been found in the associated configuration.
		/// </summary>
		/// <param name="lineIndex">Line number of the duplicate.</param>
		/// <param name="sectionIdentifier">Containing section's identifier.</param>
		void DuplicateSection(int lineIndex, string sectionIdentifier);

		/// <summary>
		/// A duplicate option within a section has been found in the associated configuration.
		/// </summary>
		/// <param name="lineIndex">Line number of the duplicate.</param>
		/// <param name="sectionIdentifier">The containing section's identifier.</param>
		/// <param name="optionIdentifier">The involved option's identifier.</param>
		void DuplicateOption(int lineIndex, string sectionIdentifier, string optionIdentifier);
	}
}
