using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Definitions
{
    /// <summary>
    /// The representation of a configuraton schema.
    /// </summary>
    public class Schema
    {
        #region Properties

        /// <summary>
        /// The list of configuration sections.
        /// </summary>
        public List<SectionDefinition> Sections { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Schema"/> class with an option to supply a collection of sections.
        /// </summary>
        /// <param name="sectionDefinitions">The definitions of sections.</param>
        public Schema(IEnumerable<SectionDefinition> sectionDefinitions = null)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
