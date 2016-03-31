using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;
using _02_api_alternative.Interfaces;

namespace _02_api_alternative
{
    /// <summary>
    /// The configuration.
    /// </summary>
    public class Configuration : IValidable
    {
        #region Properties

        public Schema Schema { get; private set; }

        /// <summary>
        /// The configuration sections.
        /// </summary>
        public Dictionary<string, Section> Sections { get; private set; }

        #endregion

        public bool IsValid(ValidationMode mode)
        {
            throw new NotImplementedException();
        }

        // Will contain an internal list of sections loaded from the stream.
    }
}
