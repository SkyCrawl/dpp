using System;
using System.Collections.Generic;
using System.Text;
using Ini.Configuration;
using Ini.EventLogs;
using Ini.Exceptions;
using YamlDotNet.Serialization;

namespace Ini.Specification
{
    /// <summary>
    /// Configuration specification. Represents configuration schema.
    /// </summary>
    public class ConfigSpec
    {
        #region Properties

        /// <summary>
        /// Path to the configuration, if any.
        /// </summary>
        [YamlMember(Alias = "origin")]
        public string Origin { get; set; }

        /// <summary>
        /// The list of configuration sections.
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

        #region Public Methods

        /// <summary>
        /// Determines whether the current content of the specification is valid.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
		/// <param name="eventLog">Schema validation event log.</param>
		public bool IsValid(ISpecValidatorEventLog eventLog)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates a default configuration from this specification. Throws
        /// an exception if the schema is not valid.
        /// </summary>
        /// <exception cref="InvalidSpecException">If the schema is not valid.</exception>
        /// <returns>The config stub.</returns>
		/// <param name="eventLog">Schema validation event log.</param>
		public Config CreateConfigStub(ISpecValidatorEventLog eventLog)
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
