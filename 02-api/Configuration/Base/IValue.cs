using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;

namespace Ini.Configuration.Base
{
    /// <summary>
    /// Interface for an option's value.
    /// </summary>
    public interface IValue : IElement
    {
        /// <summary>
        /// The element's value, cast to the output type.
        /// </summary>
        OutputType GetValue<OutputType>();
    }
}
