using System;
using System.Collections.Generic;
using Ini.Configuration;
using Ini.Configuration.Base;
using Ini.Specification;
using Ini.Validation;
using Ini.EventLoggers;
using Ini.Util.LinkResolving;
using Ini.Util;

namespace Ini.Configuration.Links
{
    /// <summary>
    /// Inclusion link.
    /// </summary>
    public class InclusionLink : ILink
    {
        #region Properties

        /// <summary>
        /// Readonly type associated with the parent option.
        /// </summary>
        /// <value>The type of the value.</value>
        public Type ValueType { get; private set; }

        /// <summary>
        /// This link's target.
        /// </summary>
        /// <value>The target.</value>
        public LinkTarget Target { get; private set; }

        /// <summary>
        /// After the link is resolved, this collection contains the link's value objects.
        /// Their type must be identical to <see cref="ValueType"/>.
        /// </summary>
        /// <value>The values.</value>
        public List<IValue> ValueObjects { get; private set; }

        /// <summary>
        /// An indicator whether this link has been resolved.
        /// </summary>
        /// <value><c>true</c> if this link has been resolved; otherwise, <c>false</c>.</value>
        public bool IsResolved { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Links.InclusionLink"/> class.
        /// </summary>
        /// <param name="valueType">The value type associated with the parent option.</param>
        /// <param name="target">This link's target.</param>
        public InclusionLink(Type valueType, LinkTarget target)
        {
            this.ValueType = valueType;
            this.Target = target;
            this.ValueObjects = new List<IValue>();
            this.IsResolved = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Looks into <see cref="Target"/> and if it's resolved, updates the inner data accordingly.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        public void Resolve(Config config)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts this element into an array of correctly typed elementary values.
        /// </summary>
        /// <returns>The values.</returns>
        public OutputType[] GetValues<OutputType>()
        {
            return ValueObjects.ToValueArray<OutputType, IValue>();
        }

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="optionSpec">The option specification.</param>
        /// <param name="mode">The validation mode.</param>
        /// <param name="eventLog">The validation event log.</param>
        /// <returns></returns>
        public bool IsValid(OptionSpec optionSpec, ConfigValidationMode mode, IConfigValidatorEventLogger eventLog = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
