using System;
using System.Collections.Generic;

namespace Ini.Schema.Elements
{
    /// <summary>
    /// The definition of a signed option.
    /// </summary>
    public class SignedOptionSpec : OptionSpec<long>
    {
        #region Properties

        /// <summary>
        /// The minimal value.
        /// </summary>
        public long MinValue { get; set; }

        /// <summary>
        /// The maximal value.
        /// </summary>
        public long MaxValue { get; set; }

        #endregion

        #region Constructor

        /// <summary>
		/// Initializes a new instance of the <see cref="SignedOptionSpec"/> class.
        /// </summary>
        public SignedOptionSpec()
        {
            MinValue = long.MinValue;
            MaxValue = long.MaxValue;
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
