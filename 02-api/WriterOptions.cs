﻿using System;
using System.Collections.Generic;

namespace Ini
{
    /// <summary>
    /// The options for writing configuration to file.
    /// </summary>
    public class WriterOptions
    {
        /// <summary>
        /// The mode of validation.
        /// </summary>
        public ValidationMode ValidationMode { get; set; }

        /// <summary>
        /// If true, than the configuration is saved to file even if it is invalid.
        /// </summary>
        public bool WriteInvalidConfiguration { get; set; }
    }
}
