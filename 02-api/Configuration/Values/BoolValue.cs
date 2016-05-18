﻿using System;
using System.Collections.Generic;
using Ini.EventLoggers;
using Ini.Specification;
using Ini.Util;
using Ini.Configuration.Base;
using Ini.Specification.Values;
using System.Linq;

namespace Ini.Configuration.Values
{
    /// <summary>
    /// Element of type <see cref="bool"/>.
    /// </summary>
    public class BoolValue : ValueBase<bool>
    {
        #region Types

        /// <summary>
        /// The formats supported by conversion to/from strings.
        /// </summary>
        public enum BoolFormat
        {
            /// <summary>
            /// True: '1'. False: '0'.
            /// </summary>
            CHARACTER_01,

            /// <summary>
            /// True: 't'. False: 'f'.
            /// </summary>
            CHARACTER_TF,

            /// <summary>
            /// True: 'y'. False: 'n'.
            /// </summary>
            CHARACTER_YN,

            /// <summary>
            /// True: 'on'. False: 'off'.
            /// </summary>
            ON_OFF,

            /// <summary>
            /// True: 'yes'. False: 'no'.
            /// </summary>
            YES_NO,

            /// <summary>
            /// True: 'enabled'. False: 'disabled'.
            /// </summary>
            ENABLED_DISABLED
        }

        #endregion

        #region Properties

        /// <summary>
        /// The original format as determined by <see cref="FillFromString"/>,
        /// or user-defined format. It is used for serialization. Default: yes/no.
        /// </summary>
        /// <value>The format.</value>
        public BoolFormat Format { get; set; }

        /// <summary>
        /// The mapping of strings that represent "true" value to their respective format.
        /// </summary>
        public static Dictionary<string, BoolFormat> TrueStrings = new Dictionary<string, BoolFormat>
        {
            { "1", BoolFormat.CHARACTER_01 },
            { "t", BoolFormat.CHARACTER_TF },
            { "y", BoolFormat.CHARACTER_YN },
            { "on", BoolFormat.ON_OFF },
            { "yes", BoolFormat.YES_NO },
            { "enabled", BoolFormat.ENABLED_DISABLED },
        };

        /// <summary>
        /// The mapping of strings that represent "false" value to their respective format.
        /// </summary>
        public static Dictionary<string, BoolFormat> FalseStrings = new Dictionary<string, BoolFormat>
        {
            { "0", BoolFormat.CHARACTER_01 },
            { "f", BoolFormat.CHARACTER_TF },
            { "n", BoolFormat.CHARACTER_YN },
            { "off", BoolFormat.ON_OFF },
            { "no", BoolFormat.YES_NO },
            { "disabled", BoolFormat.ENABLED_DISABLED },
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolValue"/> class.
        /// </summary>
        internal BoolValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolValue"/> class
        /// with an initial value.
        /// </summary>
        public BoolValue(bool value) : base(value)
        {
            this.Format = BoolFormat.YES_NO;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parses the string value and initializes the <see cref="ValueBase{T}.Value"/> property.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <exception cref="ArgumentException">If the given string value could not be interpreted.</exception>
        public override void FillFromString(string value)
        {
            // the library doesn't care about casing, as long as the base string is matched
            string lowercaseValue = value.Trim().ToLower();

            // try to parse
            if(TrueStrings.ContainsKey(lowercaseValue))
            {
                this.Value = true;
                this.Format = TrueStrings[lowercaseValue];
            }
            else if(FalseStrings.ContainsKey(lowercaseValue))
            {
                this.Value = false;
                this.Format = FalseStrings[lowercaseValue];
            }
            else
            {
                throw new ArgumentException("Unknown boolean representation: " + value);
            }
        }

        /// <summary>
        /// Serializes this element into a string that can be deserialized back using <see cref="ConfigParser"/>.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <returns>The element converted to a string.</returns>
        public override string ToOutputString(Config config)
        {
            return (this.Value ? TrueStrings : FalseStrings).GetKeysForValue(Format).Single();
        }

        #endregion

        #region Validation

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <param name="section">The current section.</param>
        /// <param name="specification">The current option's specification.</param>
        /// <param name="configLogger">Configuration validation event logger.</param>
        /// <returns></returns>
        public override bool IsValid(Config config, string section, OptionSpec specification, IConfigValidatorEventLogger configLogger)
        {
            return true;
        }

        #endregion
    }
}
