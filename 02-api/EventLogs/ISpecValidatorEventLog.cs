using System;
using System.Collections.Generic;

namespace Ini.EventLogs
{
	/// <summary>
	/// Interface providing information about validating specifications.
	/// </summary>
    public interface ISpecValidatorEventLog
    {
		/// <summary>
		/// A duplicate section has been found in the associated specification.
		/// </summary>
		/// <param name="sectionIdentifier">Containing section's identifier.</param>
		void DuplicateSection(string sectionIdentifier);

		/// <summary>
		/// A duplicate option within a section has been found in the associated specification.
		/// </summary>
		/// <param name="sectionIdentifier">The containing section's identifier.</param>
		/// <param name="optionIdentifier">The involved option's identifier.</param>
		void DuplicateOption(string sectionIdentifier, string optionIdentifier);

		/// <summary>
		/// Default value is expected for the given option.
		/// </summary>
		/// <param name="sectionIdentifier">The containing section's identifier.</param>
		/// <param name="optionIdentifier">The involved option's identifier.</param>
		void DefaultValueExpected(string sectionIdentifier, string optionIdentifier);

        /// <summary>
		/// An option's default value is invalid.
        /// </summary>
		/// <param name="sectionIdentifier">The containing section's identifier.</param>
		/// <param name="optionIdentifier">The involved option's identifier.</param>
        /// <param name="elementIndex">Index of the affected element.</param>
        /// <param name="value">The affected value.</param>
        void DefaultValueInvalid(string sectionIdentifier, string optionIdentifier, int elementIndex, object value); // TODO: a single encapsulation type instead of "value"
    }
}
