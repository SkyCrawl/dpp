using System;

namespace Ini.Util.LinkResolving
{
	/// <summary>
    /// Represents a configuration location and defines its index (key).
	/// </summary>
	public abstract class LinkBase
	{
		/// <summary>
		/// The section where the link originated.
		/// </summary>
		/// <value>The section.</value>
		public string Section { get; private set; }

		/// <summary>
		/// The option where the link originated.
		/// </summary>
		/// <value>The option.</value>
		public string Option { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Util.LinkResolving.LinkBase"/> class.
		/// </summary>
		/// <param name="section">The section where the link originated.</param>
		/// <param name="option">The option where the link originated.</param>
		public LinkBase(string section, string option)
		{
			this.Section = section;
			this.Option = option;
		}

		/// <summary>
		/// Convert the current object into an appropriate link-indexing key.
		/// </summary>
		/// <returns>The key.</returns>
		public int ToKey()
		{
			/*
			 * We're using the fact that '>' is forbidden in identifiers
			 * and hence there's no risk of unexpected collision.
			 */
			return string.Format("{0}>>{1}", Section, Option).GetHashCode();
		}

		/// <summary>
		/// Determine whether source data for the key is valid.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		public bool IsKeySourceValid()
		{
			return !string.IsNullOrEmpty(Section) && !string.IsNullOrEmpty(Option);
		}
	}
}
