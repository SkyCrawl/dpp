using System;
using System.Collections.Generic;
using Ini;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface providing information about unexpected events
    /// when trying to write a configuration.
    /// </summary>
    public interface IConfigWriterEventLogger : IConfigValidatorEventLogger
    {
        /// <summary>
        /// The configuration's schema is not valid. The operation's
        /// outcome depends on the currently used <see cref="ConfigWriterOptions"/>.
        /// </summary>
        void SpecificationNotValid();

        /// <summary>
        /// The configuration is not valid. The operation's outcome depends
        /// on the currently used <see cref="ConfigWriterOptions"/>.
        /// </summary>
        void ConfigurationNotValid();

        /// <summary>
        /// One of the links in the configuration was inconsistent with it's origin option.
        /// </summary>
        /// <param name="section">The link section</param>
        /// <param name="option">The link option</param>
        /// <param name="link">The link instance</param>
        void LinkInconsistent(string section, string option, ILink link);
    }
}
