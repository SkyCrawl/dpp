using System;
using System.Collections.Generic;
using Ini.Validation;
using Ini.Util;

namespace Ini
{
    /// <summary>
	/// Options for <see cref="ConfigWriter"/>.
    /// </summary>
    public class ConfigWriterOptions
    {
		/// <summary>
		/// Gets or sets a value indicating whether to validate the configuration
		/// by its associated specification (if defined) before writing it. The
		/// specification will also be validated. If the configuration is found
		/// to be invalid, it will NOT be written.
		/// </summary>
		/// <value><c>true</c> if configuration is to be validated; otherwise, <c>false</c>.</value>
		public bool ValidateConfig { get; set; }

        /// <summary>
        /// Validation mode to apply when validating configuration.
        /// </summary>
        public ConfigValidationMode ValidationMode { get; set; }

        /// <summary>
        /// Sort order to use when writing the configuration.
        /// </summary>
        public ConfigBlockSortOrder SortOrder { get; set; }

		/// <summary>
		/// Gets default writer options.
		/// </summary>
		/// <returns>The default.</returns>
		public static ConfigWriterOptions GetDefault()
		{
			ConfigWriterOptions result = new ConfigWriterOptions();
			result.ValidateConfig = true;
			result.ValidationMode = ConfigValidationMode.Strict;
			result.SortOrder = ConfigBlockSortOrder.InsertionOrder;
			return result;
		}
    }
}
