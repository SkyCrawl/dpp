using System;
using System.Collections.Generic;
using Ini;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface defining configuration writing events.
    /// </summary>
    public interface IConfigWriterEventLogger
    {
        /// <summary>
        /// Logger for configuration validation.
        /// </summary>
        IConfigValidatorEventLogger ValidationLogger { get; }

        /// <summary>
        /// Configuration validation was called before writing and the configuration is not valid.
        /// </summary>
        void IsNotValid();

        /// <summary>
        /// The specification must be present for selected writed options.
        /// </summary>
        void NoSpecification();
    }
}
