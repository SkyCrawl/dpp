using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Definitions
{
    /// <summary>
    /// The definition for a boolean element.
    /// </summary>
    public class BooleanElementDefinition : ElementDefinition
    {
        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration element definition.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
