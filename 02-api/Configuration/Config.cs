using System;
using System.Collections;
using System.Collections.Generic;
using Ini.Schema;

namespace Ini.Configuration
{
    /// <summary>
    /// The configuration.
    /// </summary>
    public class Config
    {
        #region Properties

		/// <summary>
		/// Path to the configuration, if any.
		/// </summary>
		public string Path { get; private set; }

        /// <summary>
        /// The configuration schema.
        /// </summary>
        public ConfigSpec Schema { get; private set; }

        /// <summary>
        /// The configuration sections.
        /// </summary>
        public Dictionary<string, Section> Sections { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
		public Config(ConfigSpec schema = null)
        {
            this.Sections = new Dictionary<string, Section>();
			this.Schema = schema;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="Configuration"/> class.
		/// </summary>
		public Config(string path, ConfigSpec schema = null)
		{
			this.Path = path;
			this.Sections = new Dictionary<string, Section>();
			this.Schema = schema;
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Verifies the integrity of the configuration definition.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="backlog"></param>
        /// <returns></returns>
        public bool IsValid(ValidationMode mode, IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        // Will contain an internal list of sections loaded from the file.
    }
}
