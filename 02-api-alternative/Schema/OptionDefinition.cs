using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniConfiguration.Elements;

namespace IniConfiguration.Schema
{
    /// <summary>
    /// The definition of a schema option.
    /// </summary>
    public abstract class OptionDefinition : DefinitionBase
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
        public abstract Option CreateOption();

        #endregion
    }

    /// <summary>
    /// The definition of a schema option.
    /// </summary>
    public abstract class OptionDefinition<T> : OptionDefinition
    {
        #region Properties

        /// <summary>
        /// Default value if the element is optional.
        /// </summary>
        public List<T> DefaultValues { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionDefinition{T}"/> class.
        /// </summary>
        public OptionDefinition()
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
