using System;
using System.Collections.Generic;
using Ini.Backlogs;

namespace Ini.Schema.Elements
{
    /// <summary>
    /// The definition of an unsigned option.
    /// </summary>
    public class UnsignedOptionSpec : OptionSpec<ulong>
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
		/// Initializes a new instance of the <see cref="UnsignedOptionSpec"/> class.
        /// </summary>
        public UnsignedOptionSpec()
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
        public override bool IsValid(ISpecValidatorBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
