using System;
using System.Collections.Generic;

namespace Ini.Backlogs
{
    /// <summary>
    /// An implementation of validation backlog that writes into console.
    /// </summary>
    public class ConsoleValidationBacklog : IValidationBacklog
    {
        #region IValidationBacklog Members

        /// <summary>
        /// A duplicate section has been found.
        /// </summary>
        /// <param name="sectionIdentifier"></param>
        public void DuplicateSection(string sectionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A duplicate option has been found.
        /// </summary>
        /// <param name="sectionIdentifier"></param>
        /// <param name="optionIdentifier"></param>
        public void DuplicateOption(string sectionIdentifier, string optionIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The option value is out of range.
        /// </summary>
        /// <param name="sectionIdentifier"></param>
        /// <param name="optionIdentifier"></param>
        /// <param name="elementIndex"></param>
        /// <param name="value"></param>
        public void ValueOutOfRange(string sectionIdentifier, string optionIdentifier, int elementIndex, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The option has invalid element count.
        /// </summary>
        /// <param name="sectionIdentifier"></param>
        /// <param name="optionIdentifier"></param>
        public void InvalidElementCount(string sectionIdentifier, string optionIdentifier)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
