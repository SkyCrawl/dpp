using Ini.Configuration.Base;
using Ini.Configuration.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Ini.Exceptions;

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
        public static Dictionary<Type, Type> TypeBinding;

        #endregion

        #region Static code block

        static ValueFactory()
        {
            TypeBinding = new Dictionary<Type, Type>();
            TypeBinding.Add(typeof(bool), typeof(BoolValue));
            TypeBinding.Add(typeof(double), typeof(DoubleValue));
            TypeBinding.Add(typeof(Enum), typeof(EnumValue));
            TypeBinding.Add(typeof(long), typeof(LongValue));
            TypeBinding.Add(typeof(string), typeof(StringValue));
            TypeBinding.Add(typeof(ulong), typeof(ULongValue));
        }

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
                    IValue result = (IValue) Activator.CreateInstance(valueObjectType, true);
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

        /// <summary>
        /// Takes the specified value's type, looks into <see cref="TypeBinding"/> for its mapped
        /// value object type, creates a new instance and feeds it the specified value. If there's
        /// no mapped value object type, throws an exception.
        /// </summary>
        /// <returns>The new value object.</returns>
        /// <param name="value">Initial value for the new value object.</param>
        /// <typeparam name="TValue">The elementary value type to use.</typeparam>
        public static ValueBase<TValue> GetValue<TValue>(TValue value)
        {
            // prepare the final value object type
            Type valueObjectType = TypeBinding.TryGetValue(typeof(TValue));

            // check if defined
            if (valueObjectType != null)
            {
                // no need to check that a correct type is created - TypeBinding is internally observed
                return (ValueBase<TValue>) Activator.CreateInstance(valueObjectType, value);
            }
            else
            {
                throw new InvalidOperationException(string.Format("Could not determine the type to create. " +
                    "Have you added a type binding for '{0}'?", typeof(TValue).ToString()));
            }
        }

        #endregion
    }
}
