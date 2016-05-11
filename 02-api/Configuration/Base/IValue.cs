using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;

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

        /// <summary>
        /// Converts the inner value into a string with the appropriate format.
        /// </summary>
        /// <returns>The value converted to a string.</returns>
        string ToStringFormat();
    }
}
