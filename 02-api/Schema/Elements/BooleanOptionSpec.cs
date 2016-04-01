﻿using System;
using System.Collections.Generic;

namespace Ini.Schema.Elements
{
    /// <summary>
    /// The definition for a boolean option.
    /// </summary>
    public class BooleanOptionSpec : OptionSpec<bool>
    {
        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
