using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;

namespace _02_api_alternative.Elements
{
    /// <summary>
    /// The element of the <see cref="Enum" /> type.
    /// </summary>
    public class EnumElement<TEnum> : Element
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        #region Properties

        /// <summary>
        /// The element value.
        /// </summary>
        public TEnum Value { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumElement{TEnum}"/> class with a specified identifier.
        /// The <see cref="EnumElementDefinition{TEnum}"/> can be specified for the reference.
        /// </summary>
        /// <param name="identifier">The element identifier.</param>
        /// <param name="definition">The element definition.</param>
        public EnumElement(string identifier, EnumElementDefinition<TEnum> definition = null)
            :base (identifier, definition)
        {
        }

        #endregion

        #region Overrides

        /// <summary>
        /// The element value.
        /// </summary>
        public override object ValueObject
        {
            get { return Value; }
            set { Value = (TEnum)value; }
        }

        /// <summary>
        /// The value type.
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(TEnum); }
        }

        /// <summary>
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        enum TestEnum
        {
            A,B,C
        }

        #endregion
    }
}
