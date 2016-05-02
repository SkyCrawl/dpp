using System;
using Ini.Configuration.Base;
using Ini.Specification;
using Ini.Validation;
using Ini.EventLoggers;
using System.Collections.Generic;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Class representing a value of an option in early stage of configuration parsing.
    /// It is necessary due to <see cref="Ini.Configuration.Values.Links.InclusionLink"/>
    /// and once all the links are resolved, instances of this class will be replaced for
    /// real data types that inherit from <see cref="ValueBase{T}"/>.
    /// <seealso cref="InterpretSelf"/>
    /// </summary>
    public class ValueStub : IValue
    {
        #region Properties

        /// <summary>
        /// Mapping of elementary value types (e.g. bool) to corresponding default data types
        /// (e.g. BoolValue). Feel free to tinker with these mappings or add your own.
        /// </summary>
        public static Dictionary<Type, Type> DefaultDataTypes = new Dictionary<Type, Type>()
        {
            { typeof(bool), typeof(BoolValue)},
            { typeof(double), typeof(DoubleValue)},
            { typeof(Enum), typeof(EnumValue)},
            { typeof(long), typeof(LongValue)},
            { typeof(string), typeof(StringValue)},
            { typeof(ulong), typeof(ULongValue)},
        };

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
        /// <see cref="DefaultDataTypes"/>.
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
        /// Converts this stub into an interpreted value object. Simply put, this method
        /// takes <see cref="DataType"/>, creates a new instance from it and feeds it
        /// <see cref="Value"/>. If the data type has not been specified, a default
        /// one is taken from <see cref="DefaultDataTypes"/>. If a default data type
        /// is not available, an exception is thrown.
        /// </summary>
        /// <returns>The interpreted value object.</returns>
        /// <exception cref="InvalidOperationException">If the data type can not be determined
        /// or doesn't inherit from <see cref="ValueBase{T}"/> where "T" inherits from
        /// <see cref="ValueType"/>.</exception>
        public IValue InterpretSelf()
        {
            // prepare the data type
            if((DataType == null) && DefaultDataTypes.ContainsKey(ValueType))
            {
                DataType = DefaultDataTypes[ValueType];
            }

            // check if still not defined
            if(DataType != null)
            {
                if(DataType.IsSubclassOf(typeof(ValueBase<ValueType>)))
                {
                    return (Activator.CreateInstance(DataType) as ValueBase<ValueType>).FromString<ValueBase<ValueType>>(Value);
                }
                else
                {
                    throw new InvalidOperationException(string.Format("The type '{0}' doesn't inherit from '{1}'.", DataType.FullName, typeof(ValueBase<ValueType>).FullName));
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("Could not determine the interpreted value's data type. " +
                    "Have you added a default data type for '{0}'?", ValueType.FullName));
            }
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
            // stubs are not supposed to be in the structure when validating
            return false;
        }

        #endregion
    }
}
