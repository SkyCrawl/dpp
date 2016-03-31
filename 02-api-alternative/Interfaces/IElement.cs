using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Definitions;
using IniConfiguration.Schema;

namespace IniConfiguration.Interfaces
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
        bool IsValid(ValidationMode mode, OptionDefinition definition, IValidationBacklog backlog = null);

        #endregion
    }

}
