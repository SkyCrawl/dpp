using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Core.Events;
using Ini.Specification.Values;
using Ini.Specification;

namespace Ini
{
	/// <summary>
	/// Type resolver for YAML deserialization.
	/// </summary>
	public class SchemaTypeResolver : INodeTypeResolver
	{
		#region Fields

		/// <summary>
        /// Contains mapping of YAML option type tags (e.g. "!String") to the type of respective
        /// specification option. See class hierarchy for <see cref="OptionSpec"/>. Feel free
        /// to tinker with these mappings or add your own.
		/// </summary>
        public static Dictionary<string, Type> OptionTypesDictionary = new Dictionary<string, Type>
		{
            { "!String", typeof(StringOptionSpec)},
            { "!Enum", typeof(EnumOptionSpec)},
            { "!Boolean", typeof(BooleanOptionSpec)},
			{ "!Float", typeof(FloatOptionSpec)},
			{ "!Signed", typeof(SignedOptionSpec)},
			{ "!Unsigned", typeof(UnsignedOptionSpec)},
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
