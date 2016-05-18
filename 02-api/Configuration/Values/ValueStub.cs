using System;
using Ini.Configuration.Base;
using Ini.Specification;
using Ini.EventLoggers;
using System.Collections.Generic;
using Ini.Util;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Representation of an option's value in early stages of configuration parsing.
    /// It is necessary due to links and once all links are resolved, instances of this
    /// class will be replaced with the final value objects that inherit from
    /// <see cref="ValueBase{T}"/>.
    /// <seealso cref="InterpretSelf"/>
    /// </summary>
    public class ValueStub : IValue
    {
        #region Properties

        /// <summary>
        /// The type of the value object (inherits from <see cref="ValueBase{T}"/>) this stub
        /// should eventually be interpreted into. The parameter "T" must be equal to <see cref="ValueType"/>.
        /// By default, this field is set to 'null' so feel free to change it. Should you wish to change
        /// this property for multiple value stubs at once, consider looking at <see cref="ValueFactory.TypeBinding"/>.
        /// </summary>
        /// <value>Type type of the value object this stub should eventually be interpreted into.</value>
        public Type ValueObjectType { get; set; }

        /// <summary>
        /// The type to eventually interpret <see cref="Value"/> with.
        /// In other words, the parent option's value type.
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
        /// <param name="valueType">The parent option's value type.</param>
        /// <param name="value">The initial string value.</param>
        public ValueStub(Type valueType, string value)
        {
            this.ValueObjectType = null;
            this.ValueType = valueType;
            this.Value = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Converts this stub into an interpreted value object.
        /// </summary>
        /// <returns>The interpreted value.</returns>
        public IValue InterpretSelf()
        {
            return ValueFactory.GetValue(ValueObjectType, ValueType, Value);
        }

        /// <summary>
        /// Do not use.
        /// </summary>
        /// <returns>Nothing.</returns>
        /// <typeparam name="OutputType">Anything.</typeparam>
        /// <exception cref="InvalidOperationException">Always.</exception>
        [Obsolete("This method throws an exception as it was inherited and doesn't hold its meaning in this special class.")]
        public OutputType GetValue<OutputType>()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// DO not use.
        /// </summary>
        /// <param name="value">Whatever.</param>
        /// <exception cref="InvalidOperationException">Always.</exception>
        [Obsolete("This method throws an exception as it was inherited and doesn't hold its meaning in this special class.")]
        public void FillFromString(string value)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Do not use.
        /// </summary>
        /// <param name="config">Whatever.</param>
        /// <param name="section">Whatever.</param>
        /// <param name="specification">Whatever.</param>
        /// <param name="configLogger">Whatever.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="InvalidOperationException">Always.</exception>
        [Obsolete("This method throws an exception as it was inherited and doesn't hold its meaning in this special class.")]
        public bool IsValid(Config config, string section, OptionSpec specification, IConfigValidatorEventLogger configLogger)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Do not use.
        /// </summary>
        /// <returns>Nothing.</returns>
        /// <param name="config"></param>
        /// <exception cref="InvalidOperationException">Always.</exception>
        [Obsolete("This method throws an exception as it was inherited and doesn't hold its meaning in this special class.")]
        public string ToOutputString(Config config)
        {
            throw new InvalidOperationException();
        }

        #endregion
    }
}
