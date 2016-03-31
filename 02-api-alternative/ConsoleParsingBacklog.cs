using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Interfaces;

namespace _02_api_alternative
{
    public class ConsoleParsingBacklog : IParsingBacklog
    {
        public void ParsingError(int lineIndex, string message)
        {
            Console.WriteLine("...");
        }

        public void ParsingException(Exception e)
        {
            Console.WriteLine("...");
        }
    }
}
