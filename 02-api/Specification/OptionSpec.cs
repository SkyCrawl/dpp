using System;
using System.Collections.Generic;
using Ini.Configuration;
using Ini.EventLoggers;
using Ini.Util;
using YamlDotNet.Serialization;

namespace Ini.Specification
{
    /// <summary>
    /// Specification for an option (see <see cref="Option"/>).
    /// </summary>
    public abstract class OptionSpec : SpecBlockBase
    {
        #region Properties

        /// <summary>
        /// True if the option only has a single value.
        /// </summary>
        [YamlMember(Alias = "single_value")]
        public bool HasSingleValue { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the type of this option's values.
        /// </summary>
        /// <returns>This option's value type.</returns>
        public abstract Type GetValueType();

        /// <summary>
        /// Creates a new option from this option specification.
        /// </summary>
        /// <returns></returns>
        public abstract Option CreateOptionStub();

        /// <summary>
        /// Determines whether the specification is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="eventLogger">Specification validation event logger.</param>
        public abstract bool IsValid(string sectionIdentifier, ISpecValidatorEventLogger eventLogger);

        #endregion
    }

    /// <summary>
    /// The definition of a schema option.
    /// </summary>
    public abstract class OptionSpec<TValue> : OptionSpec
    {
        #region Properties

        /// <summary>
        /// Default value if the element is optional.
        /// </summary>
        [YamlMember(Alias = "default_values")]
        public List<TValue> DefaultValues { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionSpec{T}"/> class.
        /// </summary>
        public OptionSpec()
        {
            this.DefaultValues = new List<TValue>();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Gets the type of this option's values.
        /// </summary>
        /// <returns>This option's value type.</returns>
        public override Type GetValueType()
        {
            return typeof(TValue);
        }

        /// <summary>
        /// Determines whether the specification is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        /// <param name="sectionIdentifier">The containing section's identifier.</param>
        /// <param name="eventLogger">Specification validation event logger.</param>
        public override bool IsValid(string sectionIdentifier, ISpecValidatorEventLogger eventLogger)
        {
            bool result = true;
            if(!IsMandatory && DefaultValues.Count == 0)
            {
                result = false;
                eventLogger.NoValue(sectionIdentifier, Identifier);
            }
            if(HasSingleValue && DefaultValues.Count > 1)
            {
                result = false;
                eventLogger.TooManyValues(sectionIdentifier, Identifier);
            }
            return result;
        }

        /// <summary>
        /// Creates a new section with empty mandatory option values and default optional option values.
        /// </summary>
        /// <returns></returns>
        public override Option CreateOptionStub()
        {
            Option result = new Option(Identifier, GetValueType(), Description);
            foreach(TValue value in DefaultValues)
            {
                result.Elements.Add(ValueFactory.GetValue<TValue>(value));
            }
            return result;
        }

        #endregion
    }
}
