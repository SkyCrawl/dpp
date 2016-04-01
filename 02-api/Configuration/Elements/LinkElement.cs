using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.Configuration.Elements
{
    public class LinkElement : IElement
    {
        #region Property

        public Option Target { get; set; }

        #endregion

        #region IElement Members

        public object ValueObject
        {
            get
            {
                return Target.ValueObject;
            }
            set
            {
                Target.ValueObject = value;
            }
        }

        public Type ValueType
        {
            get { return Target.ValueType; }
        }

        public T GetValue<T>()
        {
            return Target.GetValue<T>();
        }

        public bool IsValid(ValidationMode mode, Schema.OptionSpec definition, Backlogs.IValidationBacklog backlog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
