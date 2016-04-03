using System;

namespace Ini.Util.Guid
{
	/// <summary>
	/// Unique identifier generator using the system GUID.
	/// </summary>
	public class SystemGuidGenerator : IGuid<string>
	{
		#region IGuid implementation

		/// <summary>
		/// Returns the next unique identifier.
		/// </summary>
		public string Next()
		{
			return System.Guid.NewGuid().ToString();
		}

		#endregion
	}
}
