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
    }
}
