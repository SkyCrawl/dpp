using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;

namespace _02_api_alternative
{
    /// <summary>
    /// The class used to write a configuration into stream.
    /// </summary>
    public class ConfigurationWriter
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationWriter"/> class with an optional configuration schema.
        /// </summary>
        /// <param name="schema">The configuration schema.</param>
        public ConfigurationWriter(Schema schema = null)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes the configuration into a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="configuration"></param>
        public void Write(Stream stream, Configuration configuration)
        {
        }

        #endregion
    }
}
