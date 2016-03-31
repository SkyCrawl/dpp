using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Definitions
{
    /// <summary>
    /// The definition of a configuration section.
    /// </summary>
    public class SectionDefinition
    {
        #region Properties

        /// <summary>
        /// The section identifier.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The list of section elements.
        /// </summary>
        public List<ElementDefinition> Elements { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDefinition"/> class
        /// with an option to specify the section identifier and supply element definitions.
        /// </summary>
        /// <param name="identifier">The section identifier.</param>
        /// <param name="elements">The definition of elements.</param>
        public SectionDefinition(string identifier = null, IEnumerable<ElementDefinition> elements = null)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration section.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
