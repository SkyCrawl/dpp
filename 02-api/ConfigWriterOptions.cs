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
        #region Properties

        /// <summary>
        /// Gets the default writer options. You can access the field
        /// and change the defaults as you see fit.
        /// </summary>
        public static ConfigWriterOptions Default;

        /// <summary>
        /// Just an initializer of the <see cref="Default"/> static field.
        /// </summary>
        static ConfigWriterOptions()
        {
            Default = new ConfigWriterOptions();
            Default.ValidateConfig = true;
            Default.ValidationMode = ConfigValidationMode.Strict;
            Default.SectionSortOrder = ConfigBlockSortOrder.Insertion;
            Default.OptionSortOrder = ConfigBlockSortOrder.Insertion;
        }

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

        #endregion
    }
}
