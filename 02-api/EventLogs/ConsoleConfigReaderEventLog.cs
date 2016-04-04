using System;
using System.Collections.Generic;
using Ini.Util;
using Ini.Validation;

namespace Ini.EventLogs
{
	/// <summary>
	/// An implementation of <see cref="IConfigReaderEventLog"/> that writes into the console.
	/// </summary>
	public class ConsoleConfigReaderEventLog : ConsoleConfigValidatorEventLog, IConfigReaderEventLog
    {
        #region IParsingBacklog Members

		/// <summary>
		/// The associated reader will now parse a new configuration. Consumers
		/// will probably want to distinguish the previous output from the new.
		/// </summary>
		/// <param name="configOrigin">Origin of the newly parsed configuration.</param>
		/// <param name="schemaOrigin">Origin of the newly parsed configuration's schema.</param>
		/// <param name="mode">Validation mode applied to the configuration and schema.</param>
		public void NewConfig(string configOrigin, string schemaOrigin = null, ConfigValidationMode mode = ConfigValidationMode.Strict)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Strict validation mode was applied and no associated specification was specified.
		/// </summary>
		public void SpecNotFound()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Strict validation mode was applied and the associated specification is not valid.
		/// </summary>
		public void SpecNotValid()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// A general parsing/format error has occurred.
		/// </summary>
		/// <param name="lineIndex">Line number where the error occurred.</param>
		/// <param name="message">Message of the error.</param>
		public void ConfigMalformed(int lineIndex, string message)
		{
			throw new NotImplementedException();
		}

        #endregion
    }
}
