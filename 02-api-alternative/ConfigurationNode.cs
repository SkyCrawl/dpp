using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniConfiguration
{
    /// <summary>
    /// The common properties of the configuration nodes.
    /// </summary>
    public abstract class ConfigurationNode
    {
        #region Properties

        /// <summary>
        /// The indetifier of the configuration node.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// The commentary of the configuration node.
        /// </summary>
        public string Commentary { get; set; }

        #endregion
    }
}
