using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;

namespace _02_api_alternative
{
    /// <summary>
    /// The configuration.
    /// </summary>
    public class Configuration
    {
        #region Properties

        /// <summary>
        /// The configuration sections.
        /// </summary>
        public Dictionary<string, Section> Sections { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// If the schema is specified, than the configuration will contain mandatory sections and elements with default values.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public Configuration(Schema schema)
        {
        }

        #endregion

        // Will contain an internal list of sections loaded from the stream.
    }
}
