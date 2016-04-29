using System;

namespace Ini.Configuration.Base
{
    /// <summary>
    /// Base class with common properties and interface for blocks
    /// of the configuration.
    /// </summary>
    public abstract class ConfigBlockBase
    {
        #region Properties

        /// <summary>
        /// Identifier of the underlying block.
        /// </summary>
        public string Identifier { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Common constructor for certain configuration blocks.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        public ConfigBlockBase(string identifier)
        {
            this.Identifier = identifier;
        }

        #endregion
    }
}
