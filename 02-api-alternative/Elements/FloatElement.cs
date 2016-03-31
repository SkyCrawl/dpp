using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;

namespace _02_api_alternative.Elements
{
    /// <summary>
    /// The element of the <see cref="double" /> type.
    /// </summary>
    public class FloatElement : Element
    {
        #region Properties

        /// <summary>
        /// The element value.
        /// </summary>
        public double Value { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FloatElement"/> class with a specified identifier.
        /// The <see cref="FloatElementDefinition"/> can be specified for the reference.
        /// </summary>
        /// <param name="identifier">The element identifier.</param>
        /// <param name="definition">The element definition.</param>
        public FloatElement(string identifier, FloatElementDefinition definition = null)
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
            set { Value = (double)value; }
        }

        /// <summary>
        /// The value type.
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(double); }
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
