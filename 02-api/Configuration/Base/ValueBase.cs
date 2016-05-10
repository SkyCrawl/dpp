using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using Ini.Configuration.Values;

namespace Ini.Configuration.Base
{
    /// <summary>
    /// Base parametrized class for elements.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class ValueBase<TValue> : IValue
    {
        #region Properties

        /// <summary>
        /// The element's value.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// The type of the element's value. Must correspond
        /// to the parent option's read-only <see cref="Option.ValueType"/>.
        /// </summary>
        public Type ValueType { get { return typeof(TValue); } }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Base.ValueBase{T}"/> class.
        /// </summary>
        protected ValueBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Base.ValueBase{T}"/> class
        /// with an initial value.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public ValueBase(TValue value)
        {
            this.Value = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// The element's value, correctly typed.
        /// </summary>
        /// <typeparam name="OutputType">The correct type.</typeparam>
        /// <exception cref="System.InvalidCastException">The specified type was incorrect.</exception>
        /// <returns></returns>
        public OutputType GetValue<OutputType>()
        {
            return (OutputType) (Value as object);
        }

        /// <summary>
        /// Creates an instance of self from the given string value.
        /// </summary>
        /// <param name="value">The string.</param>
        public abstract void FillFromString(string value);

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <param name="section">The current section.</param>
        /// <param name="specification">The current option's specification.</param>
        /// <param name="configLogger">Configuration validation event logger.</param>
        /// <returns></returns>
        public abstract bool IsValid(Config config, string section, OptionSpec specification, IConfigValidatorEventLogger configLogger);

        #endregion
    }
}
