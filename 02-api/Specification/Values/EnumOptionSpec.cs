using System.Collections.Generic;
using Ini.EventLoggers;
using YamlDotNet.Serialization;

namespace Ini.Specification.Values
{
    /// <summary>
    /// The definition for an enum option.
    /// </summary>
    public class EnumOptionSpec : OptionSpec<string>
    {
        #region Properties

        /// <summary>
        /// Allowed values for enum.
        /// </summary>
        [YamlMember(Alias = "allowed_values")]
        public List<string> AllowedValues { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumOptionSpec"/> class.
        /// </summary>
        public EnumOptionSpec()
        {
            this.AllowedValues = new List<string>();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Determines whether the specification block is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="eventLogger">Specification validation event logger.</param>
        public override bool IsValid(string sectionIdentifier, ISpecValidatorEventLogger eventLogger)
        {
            bool result = base.IsValid(sectionIdentifier, eventLogger);
            if(AllowedValues.Count < 2)
            {
                result = false;
                eventLogger.NoEnumValues(sectionIdentifier, Identifier);
            }
            foreach(string value in DefaultValues)
            {
                if(!AllowedValues.Contains(value))
                {
                    result = false;
                    eventLogger.ValueNotAllowed(
                        sectionIdentifier,
                        Identifier,
                        value);
                }
            }
            return result;
        }

        #endregion
    }
}
