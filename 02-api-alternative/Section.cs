using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Definitions;
using IniConfiguration.Elements;
using IniConfiguration.Interfaces;
using IniConfiguration.Schema;

namespace IniConfiguration
{
    /// <summary>
    /// The configuration section.
    /// </summary>
    public class Section : ConfigurationNode
    {
        #region Properties

        /// <summary>
        /// The section options.
        /// </summary>
        public Dictionary<string, Option> Options { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section()
        {
            Options = new Dictionary<string, Option>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration section.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="sectionDefinition"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public bool IsValid(ValidationMode mode, SectionDefinition sectionDefinition, IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        // Will containt an internal list of options loaded from the file.
    }
}
