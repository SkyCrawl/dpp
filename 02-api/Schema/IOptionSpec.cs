using System;

namespace Ini.Schema
{
    /// <summary>
    /// The inferface for option definitions.
    /// </summary>
    public interface IOptionSpec : IValidatable
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
