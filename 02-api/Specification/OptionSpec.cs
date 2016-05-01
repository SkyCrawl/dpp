using System;
using System.Collections.Generic;
using Ini.Configuration;
using YamlDotNet.Serialization;

namespace Ini.Specification
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
        [YamlMember(Alias = "single_value")]
        public bool HasSingleValue { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the type of this option's values.
        /// </summary>
        /// <returns>This option's value type.</returns>
        public abstract Type GetValueType();

        /// <summary>
        /// Creates a new option from this option specification.
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
        [YamlMember(Alias = "default_value")]
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
        /// Gets the type of this option's values.
        /// </summary>
        /// <returns>This option's value type.</returns>
        public override Type GetValueType()
        {
            return typeof(T);
        }

        /// <summary>
        /// Creates a new section with empty mandatory option values and default optional option values.
        /// </summary>
        /// <returns></returns>
        public override Option CreateOptionStub()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
