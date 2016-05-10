using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Configuration;
using YamlDotNet.Serialization;
using System.Linq;

namespace Ini.Specification
{
    /// <summary>
    /// Specification for a section (see <see cref="Section"/>).
    /// </summary>
    public class SectionSpec : SpecBlockBase
    {
        #region Properties

        /// <summary>
        /// The list of section options.
        /// </summary>
        [YamlMember(Alias = "options")]
        public List<OptionSpec> Options { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionSpec"/> class.
        /// </summary>
        public SectionSpec()
        {
            this.Options = new List<OptionSpec>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the specified option specification.
        /// </summary>
        /// <returns>The option specification, or null if not found.</returns>
        /// <param name="identifier">Target option identifier.</param>
        public OptionSpec GetOption(string identifier)
        {
            return Options.Find(option => option.Identifier == identifier);
        }

        /// <summary>
        /// Creates a new section with default option elements.
        /// </summary>
        /// <returns></returns>
        public Section CreateSectionStub()
        {
            throw new NotImplementedException();
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
            foreach(OptionSpec optionSpec in Options)
            {
                if(validated.Contains(optionSpec.Identifier))
                {
                    // only forward the event, don't validate
                    specValid = false;
                    eventLogger.DuplicateOption(Identifier, optionSpec.Identifier);
                }
                else
                {
                    // validate
                    validated.Add(optionSpec.Identifier);
                    if(!optionSpec.IsValid(Identifier, eventLogger))
                    {
                        specValid = false;
                    }
                }
            }

            // and return
            return specValid;
        }

        #endregion
    }
}
