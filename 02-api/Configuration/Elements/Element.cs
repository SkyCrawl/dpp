using System;
using System.Collections.Generic;
using Ini.EventLogs;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// Base parametrized class for elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Element<T> : IElement
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
        /// Initializes a new instance of the <see cref="Ini.Configuration.Elements.Element{T}"/> class.
        /// </summary>
        public Element()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Elements.Element{T}"/> class
        /// with an initial value.
        /// </summary>
        /// <param name="value">The initial value.</param>
        public Element(T value)
        {
            this.Value = value;
        }

        #endregion

        #region IElement Members

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
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="optionSpec">The option specification.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="eventLog">The validation event log.</param>
        /// <returns></returns>
        public abstract bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLog eventLog = null);

        #endregion
    }
}
