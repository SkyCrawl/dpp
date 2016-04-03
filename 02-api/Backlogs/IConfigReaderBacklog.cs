using System;
using System.Collections.Generic;
using Ini.Util;
using Ini.Validation;

namespace Ini.Backlogs
{
	/// <summary>
	/// Interface providing information about reading configurations,
	/// as well validating them.
	/// </summary>
	public interface IConfigReaderBacklog : IConfigValidatorBacklog
    {
		/// <summary>
		/// The associated reader will now parse a new configuration. Consumers
		/// will probably want to distinguish the previous output from the new.
		/// </summary>
		/// <param name="configOrigin">Origin of the newly parsed configuration.</param>
		/// <param name="schemaOrigin">Origin of the newly parsed configuration's schema.</param>
		/// <param name="mode">Validation mode applied to the configuration and schema.</param>
		void NewConfig(string configOrigin, string schemaOrigin = null, ConfigValidationMode mode = ConfigValidationMode.Strict);

		/// <summary>
		/// A general parsing/format error has occurred.
		/// </summary>
		/// <param name="lineIndex">Line number where the error occurred.</param>
		/// <param name="message">Message of the error.</param>
		void ParsingError(int lineIndex, string message);
    }
}
