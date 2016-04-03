using System;

namespace Ini.Util.Guid
{
	/// <summary>
	/// General interface to create unique identifiers.
	/// </summary>
	public interface IGuid<TGuid>
	{
		/// <summary>
		/// Returns the next unique identifier.
		/// </summary>
		TGuid Next();
	}
}
