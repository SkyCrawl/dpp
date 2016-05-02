using System;
using System.Collections.Generic;
using Ini.EventLoggers;

namespace Ini.Specification.Values
{
    /// <summary>
    /// The definition for a boolean option.
    /// </summary>
    public class BooleanOptionSpec : OptionSpec<bool>
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
