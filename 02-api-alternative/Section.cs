using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Elements;

namespace _02_api_alternative
{
    /// <summary>
    /// The configuration section.
    /// </summary>
    public class Section
    {
        #region Properties

        /// <summary>
        /// Section elements.
        /// </summary>
        public Dictionary<string, Element> Elements { get; private set; }

        #endregion

        // Will containt an internal list of elements loaded from the stream.
    }
}
