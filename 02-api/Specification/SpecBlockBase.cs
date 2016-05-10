using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;
using Ini.EventLoggers;
using Ini.Util;

namespace Ini.Specification
{
    /// <summary>
    /// The base class for configuration definitions.
    /// </summary>
    public abstract class SpecBlockBase
    {
        #region Properties

        /// <summary>
        /// The definition identifier.
        /// </summary>
        [YamlMember(Alias = "identifier")]
        public string Identifier { get; set; }

        /// <summary>
        /// True if the definition is mandatory.
        /// </summary>
        [YamlMember(Alias = "mandatory")]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// The comment description of the definition.
        /// </summary>
        [YamlMember(Alias = "description")]
        public string Description { get; set; }

        #endregion
    }
}
