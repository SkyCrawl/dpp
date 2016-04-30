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
        /// A new configuration parsing task has commenced.
        /// </summary>
        /// <param name="configOrigin">Origin of the newly parsed configuration.</param>
        /// <param name="schemaOrigin">Origin of the newly parsed configuration's schema.</param>
        /// <param name="mode">Validation mode applied to the configuration and schema.</param>
        void NewConfig(string configOrigin, string schemaOrigin = null, ConfigValidationMode mode = ConfigValidationMode.Strict);

        /// <summary>
        /// Strict validation mode was applied but the parser didn't receive a specification.
        /// </summary>
        void NoSpecification();

        /// <summary>
        /// Strict validation mode was applied but the received specification was not valid.
        /// </summary>
        void SpecificationNotValid();

        /// <summary>
        /// Parser didn't know how to parse the specified line.
        /// </summary>
        /// <param name="lineIndex">Line number.</param>
        /// <param name="line">The line.</param>
        void UnknownLineSyntax(int lineIndex, string line);

        /// <summary>
        /// A duplicate section has been found.
        /// </summary>
        /// <param name="lineIndex">Line number.</param>
        /// <param name="identifier">The duplicate section identifier.</param>
        void DuplicateSection(int lineIndex, string identifier);

        /// <summary>
        /// A duplicate option has been found.
        /// </summary>
        /// <param name="lineIndex">Line number.</param>
        /// <param name="identifier">The duplicate option identifier.</param>
        void DuplicateOption(int lineIndex, string identifier);

        /// <summary>
        /// General parsing/format error,
        /// </summary>
        /// <param name="lineIndex">Line number where the error occurred.</param>
        /// <param name="message">Message of the error.</param>
        void ConfigMalformed(int lineIndex, string message);
    }
}
