using System;
using System.Collections.Generic;
using Ini;

namespace Ini.Backlogs
{
    /// <summary>
    /// Interface providing information about unexpected events
	/// when trying to write a configuration.
    /// </summary>
	public interface IConfigWriterBacklog : IConfigValidatorBacklog
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
