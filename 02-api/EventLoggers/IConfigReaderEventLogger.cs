using System;
using System.Collections.Generic;
using Ini.Util;
using Ini.Validation;
using Ini.Specification;

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
        /// A duplicate section has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="identifier">The duplicate section identifier.</param>
        void DuplicateSection(int lineNumber, string identifier);

        /// <summary>
        /// A duplicate option has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The duplicate option identifier.</param>
        void DuplicateOption(int lineNumber, string section, string option);

        /// <summary>
        /// Strict validation mode was applied but the received specification does not contain
        /// a definition for the specified section.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="identifier">The missing section identifier.</param>
        void NoSectionSpecification(int lineNumber, string identifier);

        /// <summary>
        /// Strict validation mode was applied but the received specification does not contain
        /// a definition for the specified option.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        void NoOptionSpecification(int lineNumber, string section, string option);

        /// <summary>
        /// Parser didn't know how to parse the specified line.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="line">The line.</param>
        void UnknownLineSyntax(int lineNumber, string line);

        /// <summary>
        /// Could not determine target section and option because at least
        /// one of them was not specified.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The incomplete link.</param>
        void IncompleteLinkTarget(int lineNumber, string section, string option, string link);

        /// <summary>
        /// Could not determine target section and option because too many
        /// target components were specified.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The confusing link.</param>
        void ConfusingLinkTarget(int lineNumber, string section, string option, string link);

        // TODO:

        /// <summary>
        /// The specified link's target has not been foundCould not determine target section and option because too many
        /// target components were specified.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The confusing link.</param>
        void InvalidLinkTarget(int lineNumber, string section, string option, string link);
    }
}
