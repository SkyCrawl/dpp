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
        /// Sort order to use for sections when writing the configuration. The library
        /// will make a best effort to preserve commentary blocks' positions but it can not
        /// make any promises. Commentary blocks will stick to the first section below them,
        /// as defined by the original order.
        /// </summary>
        public ConfigBlockSortOrder SectionSortOrder { get; set; }

        /// <summary>
        /// Sort order to use for options when writing the configuration. The library
        /// will make a best effort to preserve commentary blocks' positions but it can not
        /// make any promises. Commentary blocks will stick to the first option below them,
        /// as defined by the original order.
        /// </summary>
        public ConfigBlockSortOrder OptionSortOrder { get; set; }

        /// <summary>
        /// Gets default writer options.
        /// </summary>
        /// <returns>The default.</returns>
        public static ConfigWriterOptions GetDefault()
        {
            ConfigWriterOptions result = new ConfigWriterOptions();
            result.ValidateConfig = true;
            result.ValidationMode = ConfigValidationMode.Strict;
            result.SectionSortOrder = ConfigBlockSortOrder.Insertion;
            result.OptionSortOrder = ConfigBlockSortOrder.Insertion;
            return result;
        }
    }
}
