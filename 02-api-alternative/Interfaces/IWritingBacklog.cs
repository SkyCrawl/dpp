using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IniConfiguration.Interfaces
{
    /// <summary>
    /// The interface for backlog that provides information about writing configuration.
    /// </summary>
    public interface IWritingBacklog
    {
        /// <summary>
        /// The configuration if not valid.
        /// </summary>
        void ConfigurationNotValid();
    }
}
