using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniConfiguration.Interfaces
{
    /// <summary>
    /// The interface for validation of configuration definition.
    /// </summary>
    public interface IValidableDefinition
    {
        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <returns></returns>
        bool IsValid(IValidationBacklog backlog = null);
    }
}
