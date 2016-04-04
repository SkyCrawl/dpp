using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Core.Events;
using Ini.Specification.Elements;
using Ini.Specification;

namespace Ini
{
	/// <summary>
	/// Type resolver for YAML deserialization.
	/// </summary>
	public class SpecTypeResolver : INodeTypeResolver
	{
		#region Fields

		/// <summary>
		/// Contains mapping of YAML option type tags (e.g. "!String") and instances
		/// of the corresponding library type. See class hierarchy for <see cref="OptionSpec"/>.
		/// Mapping a tag to 'null' will eventually result in a null reference exception.
		/// </summary>
		public static Dictionary<string, OptionSpec> YAML_TAG_MAP = new Dictionary<string, OptionSpec>
		{
			{ "!String", new StringOptionSpec()},
			{ "!Enum", new EnumOptionSpec()},
			{ "!Boolean", new BooleanOptionSpec()},
			{ "!Float", new FloatOptionSpec()},
			{ "!Signed", new SignedOptionSpec()},
			{ "!Unsigned", new UnsignedOptionSpec()},
		};

		#endregion

		#region INodeTypeResolver Members

		/// <summary>
		/// Custom YAML type resolver that uses <see cref="YAML_TAG_MAP"/>.
		/// </summary>
		/// <param name="nodeEvent">The node to be deserialized.</param>
		/// <param name="currentType">The node's determined type.</param>
		public bool Resolve(NodeEvent nodeEvent, ref Type currentType)
		{
			OptionSpec optionInstance;
			if (nodeEvent.Tag != null && YAML_TAG_MAP.TryGetValue(nodeEvent.Tag, out optionInstance))
			{
				currentType = optionInstance.GetType();
				return true;
			}

			return false;
		}

		#endregion
	}
}
