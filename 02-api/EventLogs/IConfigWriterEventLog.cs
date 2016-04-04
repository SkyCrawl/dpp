using System;
using System.Collections.Generic;
using Ini;

namespace Ini.EventLogs
{
    /// <summary>
    /// Interface providing information about unexpected events
	/// when trying to write a configuration.
    /// </summary>
	public interface IConfigWriterEventLog : IConfigValidatorEventLog
    {
		/// <summary>
		/// The configuration's schema is not valid. The operation's
		/// outcome depends on the currently used <see cref="ConfigWriterOptions"/>.
		/// </summary>
		void SpecNotValid();

		/// <summary>
		/// The configuration is not valid. The operation's outcome depends
		/// on the currently used <see cref="ConfigWriterOptions"/>.
		/// </summary>
        void ConfigNotValid();
    }
}
