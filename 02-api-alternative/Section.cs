using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Elements;
using _02_api_alternative.Interfaces;

namespace _02_api_alternative
{
    /// <summary>
    /// The configuration section.
    /// </summary>
    public class Section : IValidable
    {
        #region Properties

        /// <summary>
        /// Section elements.
        /// </summary>
        public Dictionary<string, Option> Options { get; private set; }

        #endregion

        public bool IsValid(ValidationMode mode)
        {
            throw new NotImplementedException();
        }

        // Will containt an internal list of elements loaded from the stream.
    }
}
