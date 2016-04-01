using System;
using System.Collections.Generic;
using Ini.Backlogs;
using Ini.Schema;

namespace Ini.Configuration
{
    /// <summary>
    /// The configuration section.
    /// </summary>
    public class Section : ConfigBase
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
        public bool IsValid(ValidationMode mode, SectionSpec sectionDefinition = null, IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        // Will containt an internal list of options loaded from the file.
    }
}
