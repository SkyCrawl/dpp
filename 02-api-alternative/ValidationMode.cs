using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative
{
    public class ParsingOptions
    {
        public ValidationMode Mode  { get; set; }

        public bool ThrowOnError { get; set; }
    }

    public enum ValidationMode
    {
        Strict,
        Relaxed
    }

    public enum ExceptionHandling
    {
        Throw,
        ReturnNull,
        ReturnInvalid
    }
}
