using System;
using System.Collections.Generic;
using Ini.EventLoggers;

namespace Ini.Specification.Elements
{
    /// <summary>
    /// The definition of a string option.
    /// </summary>
    public class StringOptionSpec : OptionSpec<string>
    {
        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <param name="eventLog"></param>
        /// <returns></returns>
        public override bool IsValid(ISpecValidatorEventLogger eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
