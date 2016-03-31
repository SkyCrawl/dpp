using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Elements;

namespace _02_api_alternative.Definitions
{
    /// <summary>
    /// The definition of a schema element.
    /// </summary>
    public abstract class ElementDefinition
    {
        #region Properties

        /// <summary>
        /// The indentifier of the element.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// True is the element is mandatory.
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Default value if the element is optional.
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// The comment description of the element.
        /// </summary>
        public string Commentary { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration element definition.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

        #endregion
    }
}
