using System;
using System.Collections.Generic;

namespace Ini.Backlogs
{
    /// <summary>
	/// An implementation of <see cref="IConfigWriterBacklog"/> that writes into the console.
    /// </summary>
	public class ConsoleConfigWriterBacklog : ConsoleConfigValidatorBacklog, IConfigWriterBacklog
    {
		#region IConfigWriterBacklog implementation

		/// <summary>
		/// Specs the not valid.
		/// </summary>
		public void SpecNotValid()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Configs the not valid.
		/// </summary>
		public void ConfigNotValid()
		{
			throw new NotImplementedException();
		}

		#endregion
    }
}
