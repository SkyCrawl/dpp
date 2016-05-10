using System;
using System.Collections.Generic;
using Ini;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface defining configuration writing events.
    /// </summary>
    public interface IConfigWriterEventLogger : IConfigValidatorEventLogger
    {
        /// <summary>
        /// One of the links in the configuration was inconsistent with it's origin option.
        /// </summary>
        /// <param name="section">The link section</param>
        /// <param name="option">The link option</param>
        /// <param name="link">The link instance</param>
        void LinkInconsistent(string section, string option, ILink link);
    }
}
