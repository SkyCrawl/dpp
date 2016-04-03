using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ini.Backlogs
{
	/// <summary>
	/// Interface providing information about reading schemas as well validating them.
	/// </summary>
	public interface ISchemaReaderBacklog : ISpecValidatorBacklog
    {
		/// <summary>
		/// The associated reader will now parse a new schema. Consumers will
		/// probably want to distinguish the previous output from the new.
		/// </summary>
		/// <param name="schemaOrigin">Origin of the newly parsed schema.</param>
		void NewSpec(string schemaOrigin);

        /// <summary>
        /// Information about a general parsing error occured.
        /// </summary>
        /// <param name="message"></param>
        void ParsingError(string message);
    }
}
