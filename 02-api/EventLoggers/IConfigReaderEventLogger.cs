using System;
using System.Collections.Generic;
using Ini.Util;
using Ini.Specification;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface that defines events for configuration reading and parsing.
    /// </summary>
    public interface IConfigReaderEventLogger
    {
        /// <summary>
        /// Logger for specification validation.
        /// </summary>
        ISpecValidatorEventLogger SpecValidatiorLogger { get; }

        /// <summary>
        /// A new configuration parsing task has commenced.
        /// </summary>
        /// <param name="configOrigin">Origin of the newly parsed configuration.</param>
        /// <param name="schemaOrigin">Origin of the newly parsed configuration's specification.</param>
        /// <param name="mode">Validation mode applied to the parsing task.</param>
        void NewConfig(string configOrigin, string schemaOrigin, ConfigValidationMode mode);

        /// <summary>
        /// Strict validation mode was applied but no specification was defined.
        /// </summary>
        void NoSpecification();

        /// <summary>
        /// Strict validation mode was applied but the specification was invalid.
        /// </summary>
        void InvalidSpecification();

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
        /// Could not determine target section or option - at least one was not specified.
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

        /// <summary>
        /// The specified link's target has not been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The confusing link.</param>
        void InvalidLinkTarget(int lineNumber, string section, string option, ILink link);
    }
}
