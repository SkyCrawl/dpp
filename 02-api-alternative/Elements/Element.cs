using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;

namespace _02_api_alternative.Elements
{
    /// <summary>
    /// The configuration element.
    /// </summary>
    public abstract class Element
    {
        #region Properties

        /// <summary>
        /// The element identifier.
        /// </summary>
        public string Identifier { get; private set; }

        /// <summary>
        /// The element value.
        /// </summary>
        public abstract object Value { get; set; }

        /// <summary>
        /// The value type.
        /// </summary>
        public abstract Type ValueType { get; }

        /// <summary>
        /// The definition for the element, if schema was provided.
        /// </summary>
        public ElementDefinition Definition { get; private set; }

        /// <summary>
        /// The commentary of the element.
        /// </summary>
        public string Commentary { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Element"/> class with a specified identifier.
        /// The <see cref="ElementDefinition"/> can be specified for the reference.
        /// </summary>
        /// <param name="identifier">The element identifier.</param>
        /// <param name="definition">The element definition.</param>
        public Element(string identifier, ElementDefinition definition)
        {
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

        #endregion
    }
}
