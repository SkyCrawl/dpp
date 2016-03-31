using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniConfiguration.Definitions
{
    /// <summary>
    /// The options for writing configuration to file.
    /// </summary>
    public class WritingOptions
    {
        /// <summary>
        /// The mode of validation.
        /// </summary>
        public ValidationMode ValidationMode { get; set; }

        /// <summary>
        /// If true, than the configuration is saved to file even if it is invalid.
        /// </summary>
        public bool WriteInvalidConfiguraion { get; set; }
    }
}
