using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_api_alternative.Interfaces
{
    // Přidat validační backlog
    public interface IValidable
    {
        bool IsValid(ValidationMode mode);
    }
}
