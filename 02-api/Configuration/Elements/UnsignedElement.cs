using System;
using System.Collections.Generic;
using Ini.Backlogs;
using Ini.Schema;
using Ini.Util;
using Ini.Validation;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// Element of type <see cref="ulong"/>.
    /// </summary>
    public class UnsignedElement : Element<ulong>
    {
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Configuration.Elements.UnsignedElement"/> class.
		/// </summary>
		public UnsignedElement(ulong value) : base(value)
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
		public override bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorBacklog backlog)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
