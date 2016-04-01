using System;
using System.Collections.Generic;
using Ini.Schema;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// The option element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Element<T> : IElement
    {
        #region Properties

        /// <summary>
        /// The element value.
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region IElement Members

        /// <summary>
        /// The element value.
        /// </summary>
        public object ValueObject
        {
            get { return Value; }
            set { Value = (T)value; }
        }

        /// <summary>
        /// The type of the value.
        /// </summary>
        public Type ValueType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Return the element value cast to a certain type.
        /// </summary>
        /// <typeparam name="OutputType"></typeparam>
        /// <returns></returns>
        public OutputType GetValue<OutputType>()
        {
            return (OutputType)ValueObject;
        }

        /// <summary>
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="definition"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public abstract bool IsValid(ValidationMode mode, OptionSpec definition, IValidationBacklog backlog = null);

        #endregion
    }
}
