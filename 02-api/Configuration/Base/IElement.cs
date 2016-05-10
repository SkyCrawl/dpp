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
        /// <param name="config">The parent configuration.</param>
        /// <param name="section">The current section.</param>
        /// <param name="specification">The current option's specification.</param>
        /// <param name="configLogger">Configuration validation event logger.</param>
        /// <returns></returns>
        bool IsValid(Config config, string section, OptionSpec specification, IConfigValidatorEventLogger configLogger);
    }
}
