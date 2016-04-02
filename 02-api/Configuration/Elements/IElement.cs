using System;
using System.Collections.Generic;
using Ini.Backlogs;
using Ini.Schema;
using Ini.Util;

namespace Ini.Configuration.Elements
{
    /// <summary>
	/// Interface for an option's elements. See <see cref="Element{T}"/>.
    /// </summary>
    public interface IElement
    {
        #region Properties

        /// <summary>
		/// The type of the element's value. It is bound to the implementing
		/// class's parametrized type and thus can not be changed. Corresponds
		/// with the containing option's <see cref="Option.ElementType"/>.
        /// </summary>
        Type ValueType { get; }

		/// <summary>
		/// The element's value as an object. Must be of type <see cref="ValueType"/>.
		/// </summary>
		object ValueObject { get; }

        #endregion

        #region Public Methods

		/// <summary>
		/// The element's value, cast to the output type. Casting
		/// exceptions are not caught.
		/// </summary>
		T GetValue<T>();

		/// <summary>
		/// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="definition"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        bool IsValid(ValidationMode mode, OptionSpec definition, IValidationBacklog backlog = null);

        #endregion
    }

}
