using System;
using System.Collections;
using System.Collections.Generic;
using Ini.Util.Guid;
using Ini.Configuration.Base;
using System.IO;
using Ini.Specification;
using Ini.Util;

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
        /// Serializes this instance into the specified text writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="options">Serialization options.</param>
        /// <param name="sectionSpecification">Section specification of the current configuration block.</param>
        /// <param name="config">The parent configuration.</param>
        internal override void SerializeSelf(TextWriter writer, ConfigWriterOptions options, SectionSpec sectionSpecification, Config config)
        {
            foreach(var line in Lines)
            {
                writer.WriteLine(IniSyntax.SerializeComment(line));
            }
        }

        #endregion
    }
}
