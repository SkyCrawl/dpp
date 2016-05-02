using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using YamlDotNet.Serialization;

namespace Ini.Specification.Values
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
        [YamlMember(Alias = "min_value")]
        public long MinValue { get; set; }

        /// <summary>
        /// The maximal value.
        /// </summary>
        [YamlMember(Alias = "max_value")]
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
        /// <param name="eventLog"></param>
        /// <returns></returns>
        public override bool IsValid(ISpecValidatorEventLogger eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
