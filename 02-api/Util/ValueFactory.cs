using Ini.Configuration.Base;
using Ini.Configuration.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ini.Util
{
    /// <summary>
    /// Factory class for Value instances.
    /// </summary>
    public static class ValueFactory
    {
        #region Properties

        /// <summary>
        /// Binding of elementary value types (e.g. bool) to corresponding configuration
        /// value objects (e.g. BoolValue). Feel free to tinker with the binding or add your own.
        /// </summary>
        public static Dictionary<Type, Type> TypeBinding = new Dictionary<Type, Type>()
        {
            { typeof(bool), typeof(BoolValue)},
            { typeof(double), typeof(DoubleValue)},
            { typeof(Enum), typeof(EnumValue)},
            { typeof(long), typeof(LongValue)},
            { typeof(string), typeof(StringValue)},
            { typeof(ulong), typeof(ULongValue)},
        };

        #endregion

        #region Public Methods

        /// <summary>
        /// Takes the specified value object type, creates a new instance from it and feeds it
        /// the specified string value. If the value object type is not defined, a default
        /// from <see cref="TypeBinding"/> is taken. If even a default is not available,
        /// an exception is thrown.
        /// The value object must inherit from <see cref="ValueBase{T}"/> where "T" inherits from
        /// the specified value type.
        /// </summary>
        /// <param name="valueObjectType">The value object type to instantiate.</param>
        /// <param name="valueType">The value type to use.</param>
        /// <param name="value">The string value to interpret.</param>
        /// <returns>The interpreted value.</returns>
        /// <exception cref="InvalidOperationException">If the value object type can not be determined
        /// or doesn't inherit from <see cref="ValueBase{T}"/> where "T" inherits from 
        /// <see cref="ValueType"/>.</exception>
        public static IValue GetValue(Type valueObjectType, Type valueType, string value)
        {
            // prepare the final value object type
            if ((valueObjectType == null) && ValueFactory.TypeBinding.ContainsKey(valueType))
            {
                valueObjectType = ValueFactory.TypeBinding[valueType];
            }

            // check if still not defined
            if (valueObjectType != null)
            {
                Type genericValueObjectType = typeof(ValueBase<>).MakeGenericType(valueType);
                if (valueObjectType.IsSubclassOf(genericValueObjectType))
                {
                    var result = Activator.CreateInstance(valueObjectType) as ValueBase<object>;
                    result.FillFromString(value);
                    return result;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Value object type '{0}' doesn't inherit from '{1}'.", valueObjectType.ToString(), genericValueObjectType.ToString()));
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("Could not determine the type to interpret the value into. " +
                    "Have you added a type binding for '{0}'?", valueType.ToString()));
            }
        }

        #endregion
    }
}
