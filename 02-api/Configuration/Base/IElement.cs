using System;
using Ini.EventLoggers;
using Ini.Specification;

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

        /// <summary>
        /// Serializes this element into a string that can be deserialized back using <see cref="ConfigParser"/>.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <returns>The element converted to a string.</returns>
        string ToOutputString(Config config);
    }
}
