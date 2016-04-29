using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;

namespace Ini.Configuration.Base
{
    /// <summary>
    /// Base parametrized class for elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ValueBase<T> : IValue
    {
        #region Properties

        /// <summary>
        /// The element's value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// The type of the element's value. It is bound to the class's
        /// parametrized type and thus can not be changed. Corresponds
        /// with the containing option's <see cref="Option.ValueType"/>.
        /// </summary>
        public Type ValueType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// The element's <see cref="Value"/> as an object.
        /// </summary>
        public object ValueObject
        {
            get { return Value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Base.ValueBase{T}"/> class
        /// with an initial value.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public ValueBase(T value)
        {
            this.Value = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// The element's value, cast to the output type. Casting
        /// exceptions are not caught.
        /// </summary>
        /// <typeparam name="OutputType"></typeparam>
        /// <exception cref="System.InvalidCastException">The specified type was incorrect.</exception>
        /// <returns></returns>
        public OutputType GetValue<OutputType>()
        {
            return (OutputType) ValueObject;
        }

        /// <summary>
        /// Converts the inner value into a correctly typed array.
        /// </summary>
        /// <returns>The value, ecapsulated in an array.</returns>
        public OutputType[] GetValues<OutputType>()
        {
            return new OutputType[] { GetValue<OutputType>() };
        }

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="optionSpec">The option specification.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="eventLog">The validation event log.</param>
        /// <returns></returns>
        public abstract bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLogger eventLog = null);

        #endregion
    }
}
