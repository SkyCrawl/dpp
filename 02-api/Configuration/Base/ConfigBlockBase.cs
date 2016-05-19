using System.IO;
using Ini.Specification;

namespace Ini.Configuration.Base
{
    /// <summary>
    /// Base class with common properties and interface for blocks
    /// of the configuration.
    /// </summary>
    public abstract class ConfigBlockBase
    {
        #region Properties

        /// <summary>
        /// Identifier of the underlying block.
        /// </summary>
        public string Identifier { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Common constructor for certain configuration blocks.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        public ConfigBlockBase(string identifier)
        {
            this.Identifier = identifier;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Serializes this instance into the specified text writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="options">Serialization options.</param>
        /// <param name="sectionSpecification">Section specification of the current configuration block.</param>
        /// <param name="config">The parent configuration.</param>
        internal abstract void SerializeSelf(TextWriter writer, ConfigWriterOptions options, SectionSpec sectionSpecification, Config config);

        #endregion
    }
}
