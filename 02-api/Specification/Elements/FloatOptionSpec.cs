using System;
using System.Collections.Generic;
using Ini.Backlogs;

namespace Ini.Specification.Elements
{
    /// <summary>
    /// The definition of a float option.
    /// </summary>
    public class FloatOptionSpec : OptionSpec<double>
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
		/// Initializes a new instance of the <see cref="FloatOptionSpec"/> class.
        /// </summary>
        public FloatOptionSpec()
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
        public override bool IsValid(ISpecValidatorBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
