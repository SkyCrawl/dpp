using System;
using System.Collections.Generic;
using Ini.Util;
using Ini.Validation;
using System.IO;
using Ini.Configuration.Base;

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
            this.writer = writer;
        }

        #region IParsingBacklog Members

        /// <summary>
        /// A new configuration parsing task has commenced.
        /// </summary>
        /// <param name="configOrigin">Origin of the newly parsed configuration.</param>
        /// <param name="schemaOrigin">Origin of the newly parsed configuration's specification.</param>
        /// <param name="mode">Validation mode applied to the parsing task.</param>
        public virtual void NewConfig(string configOrigin, string schemaOrigin = null, ConfigValidationMode mode = ConfigValidationMode.Strict)
        {
            this.writer.WriteLine("...Beginning to parse configuration from origin:");
            if(configOrigin != null)
            {
                this.writer.WriteLine("\t" + configOrigin);
            }
            if(schemaOrigin != null)
            {
                this.writer.WriteLine("...Configuration will be validated against specification at:");
                this.writer.WriteLine("\t" + schemaOrigin);
            }
            else
            {
                this.writer.WriteLine("...Configuration will not be validated.");
            }
            this.writer.WriteLine("...Validation mode: " + mode.ToString());
        }

        /// <summary>
        /// Strict validation mode was applied but the parser didn't receive a specification.
        /// </summary>
        public override void NoSpecification()
        {
            this.writer.WriteLine("ERROR: strict validation mode was applied but no specification has been specified.");
        }

        /// <summary>
        /// Strict validation mode was applied but the received specification was not valid.
        /// </summary>
        public override void SpecificationNotValid()
        {
            this.writer.WriteLine("ERROR: strict validation mode was applied but the received specification was not valid.");
        }
            
        /// <summary>
        /// Parser didn't know how to parse the specified line.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="line">The line.</param>
        public virtual void UnknownLineSyntax(int lineNumber, string line)
        {
            this.writer.WriteLine(string.Format("Line {0}: unknown syntax. Content:", lineNumber));
            this.writer.WriteLine("\t" + line);
        }

        /// <summary>
        /// A duplicate section has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="identifier">The duplicate section identifier.</param>
        public virtual void DuplicateSection(int lineNumber, string identifier)
        {
            this.writer.WriteLine(string.Format("Line {0}: duplicate section ('{1}').", lineNumber, identifier));
        }

        /// <summary>
        /// A duplicate option has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The duplicate option identifier.</param>
        public virtual void DuplicateOption(int lineNumber, string section, string option)
        {
            this.writer.WriteLine(string.Format("Line {0}: duplicate option '{1}' in section '{2}'.", lineNumber, option, section));
        }

        /// <summary>
        /// Strict validation mode was applied but the received specification does not contain
        /// a definition for the specified section.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="identifier">The missing section identifier.</param>
        public virtual void NoSectionSpecification(int lineNumber, string identifier)
        {
            this.writer.WriteLine(string.Format("Line {0}: specification is missing definition for section '{1}'.", lineNumber, identifier));
        }

        /// <summary>
        /// Strict validation mode was applied but the received specification does not contain
        /// a definition for the specified option.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        public virtual void NoOptionSpecification(int lineNumber, string section, string option)
        {
            this.writer.WriteLine(string.Format("Line {0}: specification is missing definition for option '{1}' in section '{2}'.", lineNumber, option, section));
        }

        /// <summary>
        /// Could not determine target section or option - at least one was not specified.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The incomplete link.</param>
        public virtual void IncompleteLinkTarget(int lineNumber, string section, string option, string link)
        {
            this.writer.WriteLine(string.Format("Line {0}: link specifies too few target components in section '{1}' and option '{2}'. The link:", lineNumber, section, option));
            this.writer.WriteLine("\t" + link);
        }

        /// <summary>
        /// Could not determine target section and option because too many
        /// target components were specified.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The confusing link.</param>
        public virtual void ConfusingLinkTarget(int lineNumber, string section, string option, string link)
        {
            this.writer.WriteLine(string.Format("Line {0}: link specifies too many target components in section '{1}' and option '{2}'. The link:", lineNumber, section, option));
            this.writer.WriteLine("\t" + link);
        }

        /// <summary>
        /// The specified link's target has not been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The option's identifier.</param>
        /// <param name="link">The confusing link.</param>
        public virtual void InvalidLinkTarget(int lineNumber, string section, string option, ILink link)
        {
            this.writer.WriteLine(string.Format("Line {0}: link target (section '{1}' and option '{2}') not found.", lineNumber, link.Target.Section, link.Target.Option));
            this.writer.WriteLine(string.Format("\tLink defined in section '{0}' and option '{1}'.", section, option));
        }

        #endregion
    }
}
