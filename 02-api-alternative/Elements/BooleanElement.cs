using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;

namespace _02_api_alternative.Elements
{
    /// <summary>
    /// The element of the <see cref="bool" /> type.
    /// </summary>
    public class BooleanElement : Element
    {
        #region Properties

        /// <summary>
        /// The element value.
        /// </summary>
        public bool Value { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanElement"/> class with a specified identifier.
        /// The <see cref="BooleanElementDefinition"/> can be specified for the reference.
        /// </summary>
        /// <param name="identifier">The element identifier.</param>
        /// <param name="definition">The element definition.</param>
        public BooleanElement(string identifier, BooleanElementDefinition definition = null)
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
            set { Value = (bool)value; }
        }

        /// <summary>
        /// The value type.
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(bool); }
        }

        /// <summary>
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return true;
        }

        #endregion
    }
}
