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
        /// The type of the element.
        /// </summary>
        public abstract Type ElementType { get; }

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
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();

        #endregion
    }

    /// <summary>
    /// The definition of a schema element.
    /// </summary>
    public class ElementDefinition<T> : ElementDefinition
        where T : Element
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementDefinition{T}"/> class with an option to specify an element identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public ElementDefinition(string identifier = null)
        {
        }

        #endregion

        #region Overrides

        /// <summary>
        /// The type of the element.
        /// </summary>
        public override Type ElementType
        {
            get { return typeof(T); }
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
