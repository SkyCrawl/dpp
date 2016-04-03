using System;
using System.Collections.Generic;
using Ini.Configuration;

namespace Ini.Schema
{
    /// <summary>
    /// The definition of a schema option.
    /// </summary>
    public abstract class OptionSpec : SpecBlockBase
    {
        #region Properties

        /// <summary>
        /// True if the option has only single value.
        /// </summary>
        public bool HasSingleValue { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new section with empty mandatory option values and default optional option values.
        /// </summary>
        /// <returns></returns>
        public abstract Option CreateOptionStub();

        #endregion
    }

    /// <summary>
    /// The definition of a schema option.
    /// </summary>
    public abstract class OptionSpec<T> : OptionSpec
    {
        #region Properties

        /// <summary>
        /// Default value if the element is optional.
        /// </summary>
        public List<T> DefaultValues { get; set; }

        #endregion

        #region Constructor

        /// <summary>
		/// Initializes a new instance of the <see cref="OptionSpec{T}"/> class.
        /// </summary>
        public OptionSpec()
        {
            DefaultValues = new List<T>();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Creates a new section with empty mandatory option values and default optional option values.
        /// </summary>
        /// <returns></returns>
        public override Option CreateOption()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
