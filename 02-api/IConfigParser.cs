using System.IO;
using Ini.Configuration;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;

namespace Ini
{
    /// <summary>
    /// Interface for a configuration parser.
    /// </summary>
    public interface IConfigParser
    {
        /// <summary>
        /// Passes essential objects for the next parsing task.
        /// </summary>
        /// <param name="specification">The result configuration's specification.</param>
        /// <param name="configEventLog">Configuration reading event logger.</param>
        void Prepare(ConfigSpec specification, IConfigReaderEventLogger configEventLog);

        /// <summary>
        /// Perform the next parsing task.
        /// </summary>
        /// <param name="input">The input for parsing.</param>
        /// <param name="validationMode">The validation mode to use.</param>
        Config Parse(TextReader input, ConfigValidationMode validationMode);
    }
}
