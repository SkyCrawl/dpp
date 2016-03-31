using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;

namespace _02_api_alternative.Elements
{
    /// <summary>
    /// The element of the <see cref="string" /> type.
    /// </summary>
    public class StringElement : Element
    {
        #region Properties

        /// <summary>
        /// The element value.
        /// </summary>
        public string Value { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StringElement"/> class with a specified identifier.
        /// The <see cref="StringElementDefinition"/> can be specified for the reference.
        /// </summary>
        /// <param name="identifier">The element identifier.</param>
        /// <param name="definition">The element definition.</param>
        public StringElement(string identifier, StringElementDefinition definition = null)
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
            set { Value = (string)value; }
        }

        /// <summary>
        /// The value type.
        /// </summary>
        public override Type ValueType
        {
            get { return typeof(string); }
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
