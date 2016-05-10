using System;

namespace Ini.EventLoggers
{
    /// <summary>
    /// Interface defining basic configuration validation events.
    /// </summary>
    public interface IConfigValidationBase
    {
        /// <summary>
        /// Configuration can not be validated without a specification.
        /// </summary>
        void NoSpecification();

        /// <summary>
        /// Configuration can not be validated without a valid specification.
        /// </summary>
        void SpecificationNotValid();
    }
}
