﻿using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using Ini.Configuration.Base;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Element of type <see cref="double"/>.
    /// </summary>
    public class DoubleValue : ValueBase<double>
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleValue"/> class.
        /// </summary>
        internal DoubleValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleValue"/> class
        /// with an initial value.
        /// </summary>
        public DoubleValue(double value) : base(value)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates an instance of self from the given string value.
        /// </summary>
        /// <param name="value">The string.</param>
        public override void FillFromString(string value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="optionSpec">The option specification.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="eventLog">The validation event log.</param>
        /// <returns></returns>
        public override bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLogger eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
