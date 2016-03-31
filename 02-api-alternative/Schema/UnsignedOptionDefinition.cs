using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Interfaces;

namespace IniConfiguration.Schema
{
    /// <summary>
    /// The definition of an unsigned option.
    /// </summary>
    public class UnsignedOptionDefinition : OptionDefinition<ulong>
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

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedOptionDefinition"/> class.
        /// </summary>
        public UnsignedOptionDefinition()
        {
            MinValue = ulong.MinValue;
            MaxValue = ulong.MaxValue;
        }

        #endregion

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
