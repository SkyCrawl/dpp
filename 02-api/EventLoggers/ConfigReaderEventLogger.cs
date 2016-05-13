using System;
using System.Collections.Generic;
using Ini.Util;
using System.IO;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// An implementation of <see cref="IConfigReaderEventLogger"/> forwarding output to the inherited <see cref="BaseEventLogger"/>.
    /// </summary>
    public class ConfigReaderEventLogger : BaseEventLogger, IConfigReaderEventLogger
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigReaderEventLogger"/> class.
        /// </summary>
        /// <param name="writer">Output stream for configuration reading events.</param>
        /// <param name="specValidationWriter">Output stream for specification validation events.</param>
        public ConfigReaderEventLogger(TextWriter writer, TextWriter specValidationWriter = null)
            : base(writer)
        {
            SpecValidationLogger = new SpecValidatorEventLogger(specValidationWriter ?? writer);
        }

        #endregion

        #region IConfigReaderEventLogger Members

        /// <summary>
        /// Logger for specification validation.
        /// </summary>
        /// <value>The specification validation logger.</value>
        public ISpecValidatorEventLogger SpecValidationLogger { get; private set; }

        /// <summary>
        /// A new configuration parsing task has commenced.
        /// </summary>
        /// <param name="configOrigin">Origin of the newly parsed configuration.</param>
        /// <param name="schemaOrigin">Origin of the newly parsed configuration's specification.</param>
        /// <param name="mode">Validation mode applied to the parsing task.</param>
        public virtual void NewConfig(string configOrigin, string schemaOrigin = null, ConfigValidationMode mode = ConfigValidationMode.Strict)
        {
            Writer.WriteLine(new String('-', 5));
            Writer.WriteLine("...Commencing new configuration parsing task.");
            if(configOrigin != null)
            {
                Writer.WriteLine("\tOrigin: " + configOrigin);
            }
            if(schemaOrigin != null)
            {
                Writer.WriteLine("...Configuration validated against a specification.");
                Writer.WriteLine("\tOrigin: " + schemaOrigin);
            }
            else
            {
                Writer.WriteLine("...Configuration not validated.");
            }
            Writer.WriteLine("...Validation mode: " + mode.ToString());
        }

        /// <summary>
        /// Strict validation mode was applied but the parser didn't receive a specification.
        /// </summary>
        public virtual void NoSpecification()
        {
            Writer.WriteLine("ERROR: strict validation mode was applied but no specification has been specified.");
        }

        /// <summary>
        /// Strict validation mode was applied but the received specification was not valid.
        /// </summary>
        public virtual void InvalidSpecification()
        {
            Writer.WriteLine("ERROR: strict validation mode was applied but the received specification was not valid.");
        }

        /// <summary>
        /// Parser didn't know how to parse the specified line.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="line">The line.</param>
        public virtual void UnknownLineSyntax(int lineNumber, string line)
        {
            Writer.WriteLine(string.Format("Line {0}: unknown syntax. Content:", lineNumber));
            Writer.WriteLine("\t" + line);
        }

        /// <summary>
        /// A duplicate section has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="identifier">The duplicate section identifier.</param>
        public virtual void DuplicateSection(int lineNumber, string identifier)
        {
            Writer.WriteLine(string.Format("Line {0}: duplicate section ('{1}').", lineNumber, identifier));
        }

        /// <summary>
        /// A duplicate option has been found.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="section">The option's containing section's identifier.</param>
        /// <param name="option">The duplicate option identifier.</param>
        public virtual void DuplicateOption(int lineNumber, string section, string option)
        {
            Writer.WriteLine(string.Format("Line {0}: duplicate option '{1}' in section '{2}'.", lineNumber, option, section));
        }

        /// <summary>
        /// Strict validation mode was applied but the received specification does not contain
        /// a definition for the specified section.
        /// </summary>
        /// <param name="lineNumber">1-based line number of the problem.</param>
        /// <param name="identifier">The missing section identifier.</param>
        public virtual void NoSectionSpecification(int lineNumber, string identifier)
        {
            Writer.WriteLine(string.Format("Line {0}: specification is missing definition for section '{1}'.", lineNumber, identifier));
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
            Writer.WriteLine(string.Format("Line {0}: specification is missing definition for option '{1}' in section '{2}'.", lineNumber, option, section));
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
            Writer.WriteLine(string.Format("Line {0}: link specifies too few target components in section '{1}' and option '{2}'. The link:", lineNumber, section, option));
            Writer.WriteLine("\t" + link);
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
            Writer.WriteLine(string.Format("Line {0}: link specifies too many target components in section '{1}' and option '{2}'. The link:", lineNumber, section, option));
            Writer.WriteLine("\t" + link);
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
            Writer.WriteLine(string.Format("Line {0}: link target (section '{1}' and option '{2}') not found.", lineNumber, link.Target.Section, link.Target.Option));
            Writer.WriteLine(string.Format("\tLink defined in section '{0}' and option '{1}'.", section, option));
        }

        #endregion
    }
}
