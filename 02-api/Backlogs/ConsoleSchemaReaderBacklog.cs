using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.Backlogs
{
	/// <summary>
	/// An implementation of <see cref="ISchemaReaderBacklog"/> that writes into the console.
	/// </summary>
	public class ConsoleSchemaReaderBacklog : ConsoleSchemaValidatorBacklog, ISchemaReaderBacklog
    {
        #region ISpecReaderBacklog Members

        /// <summary>
        /// The associated reader will now parse a new schema. Consumers will
        /// probably want to distinguish the previous output from the new.
        /// </summary>
        /// <param name="specOrigin">Spec origin.</param>
        public void NewSpec(string specOrigin)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// A general parsing/format error has occurred.
		/// </summary>
		/// <param name="message"></param>
		public void SpecMalformed(string message)
		{
			throw new NotImplementedException();
		}

        #endregion
    }
}
