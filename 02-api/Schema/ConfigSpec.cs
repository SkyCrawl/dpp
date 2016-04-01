using System;
using System.Collections.Generic;
using Ini.Configuration;

namespace Ini.Schema
{
    /// <summary>
    /// The representation of a configuraton schema.
    /// </summary>
    public class ConfigSpec
    {
        #region Properties

		/// <summary>
		/// Path to the configuration, if any.
		/// </summary>
		public string Origin { get; set; }

		/// <summary>
        /// The list of configuration sections.
        /// </summary>
        public List<SectionSpec> Sections { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigSpec"/> class.
        /// </summary>
        public ConfigSpec()
        {
            this.Sections = new List<SectionSpec>();
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="ConfigSpec"/> class.
        /// </summary>
		public ConfigSpec(string origin = null)
            : this()
        {
			this.Origin = origin;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new configuration with default option elements.
        /// </summary>
        /// <returns></returns>
		public Config CreateConfiguration()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
