using Ini.Specification;
using System;
using System.IO;

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
        /// Writes the configuration block into the output.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="options">The output options.</param>
        /// <param name="sectionSpecification">The specification of section with the configuration block.</param>
        protected internal abstract void WriteTo(TextWriter writer, ConfigWriterOptions options, SectionSpec sectionSpecification);

        #endregion
    }
}
