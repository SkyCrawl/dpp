using System;
using Ini.Configuration.Base;
using Ini.Specification;
using Ini.Validation;
using Ini.EventLoggers;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Class representing a value of an option in early stage of configuration parsing.
    /// It is necessary due to <see cref="Ini.Configuration.Values.Links.InclusionLink"/>
    /// and once all the links are resolved, instances of this class will be replaced for
    /// real value types that inherit from <see cref="ValueBase{T}"/>.
    /// </summary>
    public class ValueStub : IValue
    {
        #region Properties

        /// <summary>
        /// The parent option's value type. Otherwise, it would refuse this instance.
        /// </summary>
        /// <value>Parent option's value type.</value>
        public Type ValueType { get; private set; }

        /// <summary>
        /// The initial string value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Values.ValueStub"/> class.
        /// </summary>
        /// <param name="type">Parent option's type.</param>
        /// <param name="value">The initial value.</param>
        public ValueStub(Type type, string value)
        {
            this.ValueType = type;
            this.Value = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Do not use.
        /// </summary>
        /// <returns>Nothing.</returns>
        /// <typeparam name="OutputType">Anything.</typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        [Obsolete("This method throws an exception as it was inherited and doesn't hold its meaning in this class.")]
        public OutputType GetValue<OutputType>()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="optionSpec">The option specification.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="eventLog">The validation event log.</param>
        /// <returns></returns>
        public bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLogger eventLog = null)
        {
            return true;
        }

        #endregion
    }
}
