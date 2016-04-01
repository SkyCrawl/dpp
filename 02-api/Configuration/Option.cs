using System;
using System.Collections.Generic;
using System.Linq;
using Ini.Schema;
using Ini.Configuration.Elements;
using Ini.Backlogs;

namespace Ini.Configuration
{
    /// <summary>
    /// The configuration option.
    /// </summary>
    public class Option : ConfigBase
    {
        #region Properties

        /// <summary>
        /// List of option elements.
        /// </summary>
        public List<IElement> Elements { get; set; }

        public Type ElementType { get; set; }

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
        public bool IsValid(ValidationMode mode, OptionSpec definition, IValidationBacklog backlog = null)
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
