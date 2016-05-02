using System;
using System.Collections.Generic;
using Ini.Util;
using Ini.Validation;
using System.IO;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="IConfigReaderEventLogger"/> that writes a text writer.
    /// </summary>
    public class ConfigReaderEventLogger : ConfigValidatorEventLogger, IConfigReaderEventLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigReaderEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public ConfigReaderEventLogger(TextWriter writer) : base(writer)
        {
        }

        #region IParsingBacklog Members

        /// <summary>
        /// The associated reader will now parse a new configuration. Consumers
        /// will probably want to distinguish the previous output from the new.
        /// </summary>
        /// <param name="configOrigin">Origin of the newly parsed configuration.</param>
        /// <param name="schemaOrigin">Origin of the newly parsed configuration's schema.</param>
        /// <param name="mode">Validation mode applied to the configuration and schema.</param>
        public void NewConfig(string configOrigin, string schemaOrigin = null, ConfigValidationMode mode = ConfigValidationMode.Strict)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Strict validation mode was applied and no associated specification was specified.
        /// </summary>
        public void NoSpecification()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Strict validation mode was applied and the associated specification is not valid.
        /// </summary>
        public void SpecificationNotValid()
        {
            throw new NotImplementedException();
        }
            
        /// <summary>
        /// The parser didn't know how to parse the specified line.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the error.</param>
        /// <param name="line">The line.</param>
        public void UnknownLineSyntax(int lineNumber, string line)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A duplicate section has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the duplicate.</param>
        /// <param name="identifier">The duplicate section identifier.</param>
        public void DuplicateSection(int lineNumber, string identifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// A duplicate option has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the duplicate.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The duplicate option identifier.</param>
        public void DuplicateOption(int lineNumber, string section, string option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Strict validation mode was applied but the received specification does not contain
        /// a definition for the specified section.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the missing section.</param>
        /// <param name="identifier">The missing section identifier.</param>
        public void NoSectionSpecification(int lineNumber, string identifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Strict validation mode was applied but the received specification does not contain
        /// a definition for the specified option.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the missing option.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The missing option identifier.</param>
        public void NoOptionSpecification(int lineNumber, string section, string option)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Could not determine target section and option because at least
        /// one of them was not specified.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The incomplete link.</param>
        public void IncompleteLinkTarget(int lineNumber, string section, string option, string link)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Could not determine target section and option because too many
        /// target components were specified.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The confusing link.</param>
        public void ConfusingLinkTarget(int lineNumber, string section, string option, string link)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The specified link's target has not been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The confusing link.</param>
        public void InvalidLinkTarget(int lineNumber, string section, string option, string link)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
