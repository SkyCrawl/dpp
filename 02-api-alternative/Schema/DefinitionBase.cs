using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Interfaces;

namespace IniConfiguration.Schema
{
    /// <summary>
    /// The base class for configuration definitions.
    /// </summary>
    public abstract class DefinitionBase : IValidableDefinition
    {
        #region Properties

        /// <summary>
        /// The definition identifier.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// True if the definition is mandatory.
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// The comment description of the definition.
        /// </summary>
        public string Commentary { get; set; }

        #endregion

        #region IValidableDefinition Members

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public abstract bool IsValid(IValidationBacklog backlog = null);

        #endregion
    }
}
