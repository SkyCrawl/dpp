using System;
using System.Collections.Generic;

namespace Ini
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
