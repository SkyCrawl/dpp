using System;
using System.Collections.Generic;
using System.Text;
using Ini.Configuration;
using Ini.EventLoggers;
using Ini.Exceptions;
using YamlDotNet.Serialization;
using System.Linq;

namespace Ini.Specification
{
    /// <summary>
    /// Object representation of a schema. In other words, specification
    /// for a configuration (see <see cref="Config"/>).
    /// </summary>
    public class ConfigSpec
    {
        #region Properties

        /// <summary>
        /// Origin of the specification (e.g. system path), if any.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// List of section specifications.
        /// </summary>
        [YamlMember(Alias = "sections")]
        public List<SectionSpec> Sections { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSpec"/> class.
        /// </summary>
        public ConfigSpec() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSpec"/> class.
        /// </summary>
        public ConfigSpec(string origin)
        {
            this.Origin = origin;
            this.Sections = new List<SectionSpec>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the specified section specification.
        /// </summary>
        /// <returns>The section specification, or null if not found.</returns>
        /// <param name="identifier">Target section identifier.</param>
        public SectionSpec GetSection(string identifier)
        {
            return Sections.Find(section => section.Identifier == identifier);
        }

        /// <summary>
        /// Gets the specified option specification.
        /// </summary>
        /// <returns>The option specification, or null if not found.</returns>
        /// <param name="sectionIdentifier">Target section identifier.</param>
        /// <param name="optionIdentifier">Target option identifier.</param>
        public OptionSpec GetOption(string sectionIdentifier, string optionIdentifier)
        {
            SectionSpec section = GetSection(sectionIdentifier);
            return section != null ? section.GetOption(optionIdentifier) : null;
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether the specification is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        /// <param name="eventLogger">Specification validation event logger.</param>
        public bool IsValid(ISpecValidatorEventLogger eventLogger)
        {
            // validate
            bool specValid = true;
            HashSet<string> validated = new HashSet<string>();
            foreach(SectionSpec sectionSpec in Sections)
            {
                if(validated.Contains(sectionSpec.Identifier))
                {
                    // only forward the event, don't validate
                    specValid = false;
                    eventLogger.DuplicateSection(sectionSpec.Identifier);
                }
                else
                {
                    // validate
                    validated.Add(sectionSpec.Identifier);
                    if(!sectionSpec.IsValid(eventLogger))
                    {
                        specValid = false;
                    }
                }
            }

            // and return
            return specValid;
        }

        /// <summary>
        /// Generates a default configuration from this specification. Throws
        /// an exception if the schema is not valid.
        /// </summary>
        /// <exception cref="InvalidSpecException">If the schema is not valid.</exception>
        /// <returns>The config stub.</returns>
        /// <param name="eventLog">Schema validation event log.</param>
        public Config CreateConfigStub(ISpecValidatorEventLogger eventLog)
        {
            if(!IsValid(eventLog))
            {
                throw new InvalidSpecException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
