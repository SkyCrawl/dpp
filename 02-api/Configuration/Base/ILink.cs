using System;
using Ini.Util.LinkResolving;
using Ini.EventLoggers;
using Ini.Util;

namespace Ini.Configuration.Base
{
    /// <summary>
    /// Interface common for all link implementations.
    /// </summary>
    public interface ILink : IElement
    {
        /// <summary>
        /// This link's target.
        /// </summary>
        /// <value>The target.</value>
        LinkTarget Target { get; }

        /// <summary>
        /// This link's resolved values.
        /// </summary>
        /// <value>The values.</value>
        ObservableList<IValue> Values { get; }

        /// <summary>
        /// An indicator whether this link has been resolved.
        /// </summary>
        /// <value><c>true</c> if this link has been resolved; otherwise, <c>false</c>.</value>
        bool IsResolved { get; }

        /// <summary>
        /// Looks into <see cref="Target"/> and if it's resolved, updates the inner data accordingly.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <param name="configEventLogger">Logger to use if something goes wrong.</param>
        void Resolve(Config config, IConfigReaderEventLogger configEventLogger);

        /// <summary>
        /// Interprets <see cref="Values"/>, according to <see cref="ValueType"/>.
        /// </summary>
        void InterpretSelf();
    }
}
