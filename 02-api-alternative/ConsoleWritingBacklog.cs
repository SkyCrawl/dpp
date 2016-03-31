using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Interfaces;

namespace _02_api_alternative
{
    public class ConsoleWritingBacklog : IWritingBacklog
    {
        #region IWritingBacklog Members

        public void ConfigurationNotValid()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
