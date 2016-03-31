using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Elements
{
    public class Option
    {
        public string Identifier {get;set;}

        /// <summary>
        /// The commentary of the element.
        /// </summary>
        public string Commentary { get; set; }
        
        public List<Element> Elements { get; set; }
    }


}
