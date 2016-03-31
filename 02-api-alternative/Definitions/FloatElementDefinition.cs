using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Definitions
{
    /// <summary>
    /// The definition of a float element.
    /// </summary>
    public class FloatElementDefinition : ElementDefinition
    {
        #region Properties

        /// <summary>
        /// The minimal value.
        /// </summary>
        public double MinValue { get; set; }

        /// <summary>
        /// The maximal value.
        /// </summary>
        public double MaxValue { get; set; }

        #endregion

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
