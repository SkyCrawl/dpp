using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Definitions
{
    /// <summary>
    /// The definition of an unsigned element.
    /// </summary>
    public class UnsignedElementDefinition : ElementDefinition
    {
        #region Properties

        /// <summary>
        /// The minimal value.
        /// </summary>
        public ulong MinValue { get; set; }

        /// <summary>
        /// The maximal value.
        /// </summary>
        public ulong MaxValue { get; set; }

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
