using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Interfaces;

namespace IniConfiguration.Schema
{
    /// <summary>
    /// The definition of a string option.
    /// </summary>
    public class StringOptionDefinition : OptionDefinition<string>
    {
        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
