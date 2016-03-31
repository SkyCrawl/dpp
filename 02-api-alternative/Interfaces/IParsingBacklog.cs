using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Interfaces
{
    public interface IParsingBacklog
    {
        // TODO: interface might be more elaborate...
        void ParsingError(int lineIndex, string message);
        void DuplicateSection(int lineIndex, string sectionName);
    }
}
