using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniConfiguration.Schema
{
    /// <summary>
    /// The definition of a configuration section.
    /// </summary>
    public class SectionDefinition : DefinitionBase
    {
        #region Properties

        /// <summary>
        /// The list of section options.
        /// </summary>
        public List<OptionDefinition> Options { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionDefinition"/> class.
        /// </summary>
        public SectionDefinition()
        {
            Options = new List<OptionDefinition>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new section with default option elements.
        /// </summary>
        /// <returns></returns>
        public Section CreateSection()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration section definition.
        /// </summary>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(Interfaces.IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
