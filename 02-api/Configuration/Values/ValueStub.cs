using System;
using Ini.Configuration.Base;
using Ini.Specification;
using Ini.EventLoggers;
using System.Collections.Generic;
using Ini.Util;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Class representing a value of an option in early stage of configuration parsing.
    /// It is necessary due to <see cref="Ini.Configuration.Values.Links.InclusionLink"/>
    /// and once all the links are resolved, instances of this class will be replaced for
    /// real data types that inherit from <see cref="ValueBase{T}"/>. As such, any attempt
    /// to call <see cref="ValueStub.IsValid"/> results in an exception.
    /// <seealso cref="InterpretSelf"/>
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

        /// <summary>
        /// Type of the value interpreted form this stub. Must be a subclass of <see cref="ValueBase{T}"/>
        /// where the parameter "T" is <see cref="ValueType"/>. This property ensures custom transformations of
        /// individual values, in the interest of extensibility. For custom transformations for all stubs alike, see
        /// <see cref="ValueFactory.DefaultDataTypes"/>.
        /// <seealso cref="InterpretSelf"/>
        /// </summary>
        /// <value>Type of the value interpreted form this stub.</value>
        public Type DataType { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Values.ValueStub"/> class.
        /// </summary>
        /// <param name="valueType">Final elementary type of the value (i.e. parent option's value type).</param>
        /// <param name="value">The initial value.</param>
        public ValueStub(Type valueType, string value)
        {
            this.ValueType = valueType;
            this.Value = value;
            this.DataType = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Converts this stub into an interpreted value object. Simply put, this method
        /// takes <see cref="DataType"/>, creates a new instance from it and feeds it
        /// <see cref="Value"/>. If the data type has not been specified, a default
        /// one is taken from <see cref="ValueFactory.DefaultDataTypes"/>. If a default data type
        /// is not available, an exception is thrown.
        /// </summary>
        /// <returns>The interpreted value object.</returns>
        /// <exception cref="InvalidOperationException">If the data type can not be determined
        /// or doesn't inherit from <see cref="ValueBase{T}"/> where "T" inherits from
        /// <see cref="ValueType"/>.</exception>
        public IValue InterpretSelf()
        {
            return ValueFactory.GetValue(DataType, ValueType, Value);
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
        /// <returns>Nothingg.</returns>
        /// <exception cref="InvalidOperationException">Always.</exception>
        [Obsolete("This method throws an exception as it was inherited and doesn't hold its meaning in this special class.")]
        public string ToOutputString()
        {
            throw new InvalidOperationException();
        }

        #endregion
    }
}
