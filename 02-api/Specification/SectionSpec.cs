using System;
using System.Collections.Generic;
using Ini.EventLogs;
using Ini.Configuration;
using YamlDotNet.Serialization;

namespace Ini.Specification
{
    /// <summary>
    /// The definition of a configuration section.
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
            Options = new List<OptionSpec>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new section with default option elements.
        /// </summary>
        /// <returns></returns>
        public Section CreateSectionStub()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration section definition.
        /// </summary>
        /// <param name="eventLog"></param>
        /// <returns></returns>
        public override bool IsValid(ISpecValidatorEventLog eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
