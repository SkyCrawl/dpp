using System;
using System.Collections.Generic;
using Ini.Schema;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// The configuration element.
    /// </summary>
    public interface IElement
    {
        #region Properties

        /// <summary>
        /// The element value.
        /// </summary>
        object ValueObject { get; set; }

        /// <summary>
        /// The type of the value.
        /// </summary>
        Type ValueType { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return the element value cast to a certain type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetValue<T>();

        /// <summary>
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="definition"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        bool IsValid(ValidationMode mode, OptionSpec definition, IValidationBacklog backlog = null);

        #endregion
    }

}
