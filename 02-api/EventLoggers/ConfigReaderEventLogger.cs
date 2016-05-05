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

        #region Properties

        ///// <summary>
        ///// The output stream to write event logs to.
        ///// </summary>
        //protected TextWriter writer;

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigReaderEventLogger"/> class.
        /// </summary>
        /// <param name="writer">The output stream to write event logs to.</param>
        public ConfigReaderEventLogger(TextWriter writer) : base(writer)
        {
            this.writer = writer;
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
            this.writer.WriteLine("Initializing configuration parsing...");
            this.writer.WriteLine("Configuration read from " + configOrigin);

            if(schemaOrigin != null)
            {
                this.writer.WriteLine("Configuration verified against " + schemaOrigin);
                this.writer.Write("Validation mode is ");
                               
                if(mode == ConfigValidationMode.Strict)
                {
                    this.writer.WriteLine("strict");
                }
                else
                {
                    this.writer.WriteLine("relaxed");
                }
            }
            else
            {
                NoSpecification();
            }
        }

        /// <summary>
        /// No associated specification was specified.
        /// </summary>
        public void NoSpecification()
        {
            this.writer.WriteLine("No schema has been supplied for verifying configuration.");
        }

        /// <summary>
        /// Specification associated with the configuration is not valid.
        /// </summary>
        public void SpecificationNotValid()
        {
            this.writer.WriteLine("Specification associated with the configuration is not valid.");

        }
            
        /// <summary>
        /// The parser didn't know how to parse the specified line.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the error.</param>
        /// <param name="line">The line.</param>
        public void UnknownLineSyntax(int lineNumber, string line)
        {
            this.writer.WriteLine("Unknown syntax at line: " + lineNumber);
            this.writer.WriteLine(line);
        }

        /// <summary>
        /// A duplicate section has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the duplicate.</param>
        /// <param name="identifier">The duplicate section identifier.</param>
        public void DuplicateSection(int lineNumber, string identifier)
        {
            this.writer.WriteLine("Duplicate section " + identifier + " found at line: " + lineNumber);
        }

        /// <summary>
        /// A duplicate option has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the duplicate.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The duplicate option identifier.</param>
        public void DuplicateOption(int lineNumber, string section, string option)
        {
            this.writer.WriteLine("Duplicate option " + option + " in section " + section + " found at line: " + lineNumber);
        }

        /// <summary>
        /// Strict validation mode was applied but the received specification does not contain
        /// a definition for the specified section.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the missing section.</param>
        /// <param name="identifier">The missing section identifier.</param>
        public void NoSectionSpecification(int lineNumber, string identifier)
        {
            this.writer.WriteLine("Specification associated with the configuration does not contain a definition for section: " + identifier + " found at line: " + lineNumber);

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
            this.writer.WriteLine("Specification associated with the configuration does not contain a definition for option: " + option + "in section " + section + " found at line: " + lineNumber);
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
            this.writer.WriteLine("Could not determine target of option " + option + " in section " + section);
            this.writer.WriteLine("The specified link " + link + " at line: " + lineNumber + " is incomplete");
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
            this.writer.WriteLine("Could not determine target of option " + option + " in section " + section);
            this.writer.WriteLine("The specified link " + link + " at line: " + lineNumber + " contains too many components");

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
            this.writer.WriteLine("Could not determine target of option " + option + " in section " + section);
            this.writer.WriteLine("The link " + link + " at line: " + lineNumber + " does not specify an existing target");

        }

        #endregion
    }
}
