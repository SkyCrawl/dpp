using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Definitions;
using IniConfiguration.Interfaces;
using IniConfiguration.Schema;

namespace IniConfiguration
{
    /// <summary>
    /// The configuration.
    /// </summary>
    public class Configuration
    {
        #region Properties

        /// <summary>
        /// The configuration schema.
        /// </summary>
        public ConfigurationDefinition Schema { get; private set; }

        /// <summary>
        /// The configuration sections.
        /// </summary>
        public Dictionary<string, Section> Sections { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            Sections = new Dictionary<string, Section>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public bool IsValid(ValidationMode mode, IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        // Will contain an internal list of sections loaded from the file.
    }
}
