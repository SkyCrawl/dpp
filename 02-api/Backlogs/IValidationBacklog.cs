using System;
using System.Collections.Generic;

namespace Ini
{
    /// <summary>
    /// The interface for backlog that provides information about validation.
    /// </summary>
    public interface IValidationBacklog
    {
        /// <summary>
        /// A duplicate section has been found.
        /// </summary>
        /// <param name="sectionIdentifier"></param>
        void DuplicateSection(string sectionIdentifier);

        /// <summary>
        /// A duplicate option has been found.
        /// </summary>
        /// <param name="sectionIdentifier"></param>
        /// <param name="optionIdentifier"></param>
        void DuplicateOption(string sectionIdentifier, string optionIdentifier);

        /// <summary>
        /// The option value is out of range.
        /// </summary>
        /// <param name="sectionIdentifier"></param>
        /// <param name="optionIdentifier"></param>
        /// <param name="elementIndex"></param>
        /// <param name="value"></param>
        void ValueOutOfRange(string sectionIdentifier, string optionIdentifier, int elementIndex, object value);

        /// <summary>
        /// The option has invalid element count.
        /// </summary>
        /// <param name="sectionIdentifier"></param>
        /// <param name="optionIdentifier"></param>
        void InvalidElementCount(string sectionIdentifier, string optionIdentifier);
    }
}
