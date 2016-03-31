using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;
using _02_api_alternative.Interfaces;

namespace _02_api_alternative.Elements
{
    /// <summary>
    /// The configuration element.
    /// </summary>
    public interface IElement : IValidable
    {
        object ValueObject { get; set; }
        T GetValue<T>();
        Type ValueType;
    }

    public abstract class Element<T> : IElement
    {
        #region Properties

        /// <summary>
        /// The element value.
        /// </summary>
        public object ValueObject
        {
            get { return Value; }
            set { Value = (T)value; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Return the element value cast to a certain type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            return (T)ValueObject;
        }

        #endregion

        #region IValidable Members

        public abstract bool IsValid(ValidationMode mode);

        #endregion
        
        public T Value { get; set; }

        public override Type ValueType
        {
            get { return typeof(T); }
        }
    }
}
