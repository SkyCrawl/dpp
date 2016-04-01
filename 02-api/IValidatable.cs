using System;
using System.Collections.Generic;
using Ini.Backlogs;

namespace Ini
{
    /// <summary>
    /// The interface for validation of configuration definition.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <returns></returns>
        bool IsValid(IValidationBacklog backlog = null);
    }
}
