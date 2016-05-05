using System;
using System.Collections;
using System.Collections.Generic;
using Ini.Util.Guid;
using Ini.Configuration.Base;
using System.IO;
using Ini.Specification;

namespace Ini.Configuration
{
	/// <summary>
	/// Represents a block of empty and commentary lines.
	/// </summary>
	public class Commentary : ConfigBlockBase, IEnumerable<string>
	{
		#region Properties
		/// <summary>
		/// A block of empty and commentary lines.
		/// </summary>
		public List<string> Lines { get; private set; }

		/// <summary>
		/// Static identifier generator for instances of this class.
		/// </summary>
		public static IGuid<string> identifierGenerator = new SystemGuidGenerator();

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Commentary"/> class.
		/// </summary>
        public Commentary(IEnumerable<string> lines) : base(identifierGenerator.Next())
		{
			this.Lines = new List<string>(lines);
		}

		#endregion

		#region IEnumerable implementation

		/// <summary>
		/// Gets the content enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<string> GetEnumerator()
		{
			return Lines.GetEnumerator();
		}

		/// <summary>
		/// Gets the content enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Lines.GetEnumerator();
		}

        #endregion

        #region ConfigBlockBase Members

        /// <summary>
        /// Writes the commentary into the output.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="options">The output options.</param>
        /// <param name="sectionSpecification">The specification of section with the configuration block.</param>
        protected internal override void WriteTo(TextWriter writer, ConfigWriterOptions options, SectionSpec sectionSpecification)
        {
            foreach(var line in Lines)
            {
                ConfigWriter.WriteComment(writer, line);
            }
        }

        #endregion
    }
}
