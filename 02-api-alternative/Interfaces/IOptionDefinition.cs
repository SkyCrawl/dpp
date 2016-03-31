using System;
using IniConfiguration.Interfaces;
namespace IniConfiguration.Definitions
{
    /// <summary>
    /// The inferface for option definitions.
    /// </summary>
    public interface IOptionDefinition : IValidableDefinition
    {
        #region Properties

        /// <summary>
        /// The definition identifier.
        /// </summary>
        string Identifier { get; set; }

        /// <summary>
        /// True if the definition is mandatory.
        /// </summary>
        bool IsMandatory { get; set; }

        /// <summary>
        /// The comment description of the definition.
        /// </summary>
        string Commentary { get; set; }

        /// <summary>
        /// The option has a single value.
        /// </summary>
        bool HasSingleValue { get; set; }

        #endregion
    }
}
