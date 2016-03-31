﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Definitions;
using IniConfiguration.Interfaces;
using IniConfiguration.Schema;

namespace IniConfiguration
{
    /// <summary>
    /// The configuration option.
    /// </summary>
    public class Option : ConfigurationNode
    {
        #region Properties

        /// <summary>
        /// List of option elements.
        /// </summary>
        public List<IElement> Elements { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class.
        /// </summary>
        public Option()
        {
            Elements = new List<IElement>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration option.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="definition"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public bool IsValid(ValidationMode mode, OptionDefinition definition, IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retuns the typed option value for options with single element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetValue<T>()
        {
            return Elements
                .Single()
                .GetValue<T>();
        }

        /// <summary>
        /// Returns the array typed option values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] GetValues<T>()
        {
            return Elements
                .Select(item => item.GetValue<T>())
                .ToArray();
        }

        #endregion
    }
}
