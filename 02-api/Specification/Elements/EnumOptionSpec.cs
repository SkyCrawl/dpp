﻿using System;
using System.Collections.Generic;
using Ini.Backlogs;
using YamlDotNet.Serialization;

namespace Ini.Specification.Elements
{
    /// <summary>
    /// The definition for an enum option.
    /// </summary>
    public class EnumOptionSpec : OptionSpec<string>
    {
        #region Properties

        /// <summary>
        /// Allowed values for enum.
        /// </summary>
        [YamlMember(Alias = "allowed_values")]
        public List<string> AllowedValues { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumOptionSpec"/> class.
        /// </summary>
        public EnumOptionSpec()
        {
            AllowedValues = new List<string>();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public override bool IsValid(ISpecValidatorBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
