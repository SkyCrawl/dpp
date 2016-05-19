using System;
using System.Collections.Generic;
using Ini.Specification;
using Ini.Specification.Values;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Ini
{
    /// <summary>
    /// Type resolver for YAML deserialization.
    /// </summary>
    public class SpecTypeResolver : INodeTypeResolver
    {
        #region Fields

        /// <summary>
        /// Contains mapping of YAML option type tags (e.g. "!String") to the type of respective
        /// specification option. See class hierarchy for <see cref="OptionSpec"/>. Feel free
        /// to tinker with these mappings or add your own.
        /// </summary>
        public static Dictionary<string, Type> OptionTypesDictionary = new Dictionary<string, Type>()
        {
            { "!String", typeof(StringOptionSpec) },
            { "!Enum", typeof(EnumOptionSpec) },
            { "!Boolean", typeof(BooleanOptionSpec) },
            { "!Float", typeof(DoubleOptionSpec) },
            { "!Signed", typeof(LongOptionSpec) },
            { "!Unsigned", typeof(ULongOptionSpec) },
        };

        #endregion

        #region INodeTypeResolver Members

        /// <summary>
        /// Custom YAML type resolver that uses <see cref="OptionTypesDictionary"/>.
        /// </summary>
        /// <param name="nodeEvent">The node to be deserialized.</param>
        /// <param name="currentType">The node's determined type.</param>
        public bool Resolve(NodeEvent nodeEvent, ref Type currentType)
        {
            if(nodeEvent.Tag != null)
            {
                return OptionTypesDictionary.TryGetValue(nodeEvent.Tag, out currentType);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
