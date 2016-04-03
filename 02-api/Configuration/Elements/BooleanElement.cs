using System;
using System.Collections.Generic;
using Ini.Backlogs;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;

namespace Ini.Configuration.Elements
{
    /// <summary>
	/// Element of type <see cref="bool"/>.
    /// </summary>
    public class BooleanElement : Element<bool>
    {
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Configuration.Elements.BooleanElement"/> class.
		/// </summary>
		public BooleanElement(bool value) : base(value)
		{
		}

		#endregion

        #region Validation

        /// <summary>
		/// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="mode"></param>
		/// <param name="optionSpec"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
		public override bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
