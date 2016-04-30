using System;
using Ini.Specification;
using Ini.Validation;
using Ini.EventLoggers;

namespace Ini.Configuration.Base
{
    /// <summary>
    /// The absolute base interface of all elementary configuration elements.
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Readonly type associated with the parent option.
        /// </summary>
        Type ValueType { get; }

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="optionSpec">The option specification.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="eventLog">The validation event log.</param>
        /// <returns></returns>
        bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLogger eventLog = null);
    }
}
