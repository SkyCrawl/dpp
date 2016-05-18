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
        /// Creates an instance of self from the given string value.
        /// </summary>
        /// <param name="value">The value.</param>
        void FillFromString(string value);
    }
}
