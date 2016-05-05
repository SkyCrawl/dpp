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

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates an instance of a value class based of supplied types.
        /// </summary>
        /// <param name="dataType">The final type of the value.</param>
        /// <param name="valueType">The inner type of the value.</param>
        /// <param name="value">The string representation of the value.</param>
        /// <returns></returns>
        public static IValue GetValue(Type dataType, Type valueType, string value)
        {
            // prepare the data type
            if ((dataType == null) && ValueFactory.DefaultDataTypes.ContainsKey(valueType))
            {
                dataType = ValueFactory.DefaultDataTypes[valueType];
            }

            // check if still not defined
            if (dataType != null)
            {
                if (dataType.IsSubclassOf(typeof(ValueBase<ValueType>)))
                {
                    var result = Activator.CreateInstance(dataType) as ValueBase<ValueType>;
                    result.FillFromString(value);

                    return result;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("The type '{0}' doesn't inherit from '{1}'.", dataType.FullName, typeof(ValueBase<ValueType>).FullName));
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("Could not determine the interpreted value's data type. " +
                    "Have you added a default data type for '{0}'?", valueType.FullName));
            }
        }

        #endregion
    }
}
