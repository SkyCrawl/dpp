using System;
using System.Collections.Generic;
using Ini;
using Ini.Configuration.Base;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface that defines events for configuration writing.
    /// </summary>
    public interface IConfigWriterEventLogger
    {
        /// <summary>
        /// Logger for configuration validation.
        /// </summary>
        IConfigValidatorEventLogger ConfigValidatiorLogger { get; }

        /// <summary>
        /// The task's options instructed to use a specification for writing, but the configuration didn't have an associated specification.
        /// </summary>
        void NoSpecification();

        /// <summary>
        /// The task's options instructed to validate the configuration before writing, and it was found to be invalid.
        /// </summary>
        void InvalidConfiguration();
    }
}
