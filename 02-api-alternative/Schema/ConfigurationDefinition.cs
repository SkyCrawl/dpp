using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniConfiguration.Schema
{
    /// <summary>
    /// The representation of a configuraton schema.
    /// </summary>
    public class ConfigurationDefinition
    {
        #region Properties

        /// <summary>
        /// The list of configuration sections.
        /// </summary>
        public List<SectionDefinition> Sections { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDefinition"/> class.
        /// </summary>
        public ConfigurationDefinition()
        {
            Sections = new List<SectionDefinition>();
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

        /// <summary>
        /// Creates a new configuration with default option elements.
        /// </summary>
        /// <returns></returns>
        public Configuration CreateConfiguration()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
