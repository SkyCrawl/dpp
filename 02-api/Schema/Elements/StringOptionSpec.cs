using System;
using System.Collections.Generic;
using Ini.Backlogs;

namespace Ini.Schema.Elements
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
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(ISpecValidatorBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
