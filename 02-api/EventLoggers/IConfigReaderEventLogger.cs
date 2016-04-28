using System;
using System.Collections.Generic;
using Ini.Util;
using Ini.Validation;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface providing information about reading configurations.
    /// </summary>
    public interface IConfigReaderEventLogger
    {
        /// <summary>
        /// The associated reader will now parse a new configuration. Consumers
        /// will probably want to distinguish the previous output from the new.
        /// </summary>
        /// <param name="configOrigin">Origin of the newly parsed configuration.</param>
        /// <param name="schemaOrigin">Origin of the newly parsed configuration's schema.</param>
        /// <param name="mode">Validation mode applied to the configuration and schema.</param>
        void NewConfig(string configOrigin, string schemaOrigin = null, ConfigValidationMode mode = ConfigValidationMode.Strict);

        /// <summary>
        /// Strict validation mode was applied and no associated specification was specified.
        /// </summary>
        void SpecNotFound();

        /// <summary>
        /// Strict validation mode was applied and the associated specification is not valid.
        /// </summary>
        void SpecNotValid();

        /// <summary>
        /// A general parsing/format error has occurred.
        /// </summary>
        /// <param name="lineIndex">Line number where the error occurred.</param>
        /// <param name="message">Message of the error.</param>
        void ConfigMalformed(int lineIndex, string message);
    }
}
