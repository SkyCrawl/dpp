using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;

namespace _02_api_alternative.Elements
{
    /// <summary>
    /// The element of the <see cref="ulong" /> type.
    /// </summary>
    public class UnsignedElement : Element
    {
        #region Properties

        /// <summary>
        /// The element value.
        /// </summary>
        public ulong Value { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsignedElement"/> class with a specified identifier.
        /// The <see cref="UnsignedElementDefinition"/> can be specified for the reference.
        /// </summary>
        /// <param name="identifier">The element identifier.</param>
        /// <param name="definition">The element definition.</param>
        public UnsignedElement(string identifier, UnsignedElementDefinition definition = null)
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
            set { Value = (ulong)value; }
        }

        /// <summary>
        /// The value type.
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(ulong); }
        }

        /// <summary>
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
