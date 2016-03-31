using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Interfaces;

namespace IniConfiguration.Schema
{
    /// <summary>
    /// The definition of a float option.
    /// </summary>
    public class FloatOptionDefinition : OptionDefinition<double>
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

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatOptionDefinition"/> class.
        /// </summary>
        public FloatOptionDefinition()
        {
            MinValue = double.MinValue;
            MaxValue = double.MaxValue;
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
