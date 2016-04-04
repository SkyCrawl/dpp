using System;
using System.Collections.Generic;

namespace Ini.Validation
{
    /// <summary>
    /// Validation mode for configurations.
    /// </summary>
    public enum ConfigValidationMode
    {
        /// <summary>
        /// Strict validation against an associated specification, as requested by client.
        /// </summary>
        Strict,
        /// <summary>
		/// Relaxed mode without an associated specification, as requested by client.
        /// </summary>
        Relaxed
    }
}
