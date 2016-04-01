using System;
using System.Collections.Generic;
using Ini.Schema;

namespace Ini.Configuration.Elements
{
    /// <summary>
    /// The element of the <see cref="string" /> type.
    /// </summary>
    public class StringElement : Element<string>
    {
        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration element.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="definition"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(ValidationMode mode, OptionSpec definition, IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
