using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini.Specification;
using Ini.Specification.Elements;
using NUnit.Framework;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace apitest
{
    [TestFixture]
    public class TestSpec
    {
        [Test()]
        public void TestSpecSerialization()
        {
            var file = File.OpenText("Examples\\config.yml");
            var specString = file.ReadToEnd();

            var deserializer = new Deserializer();
            deserializer.TypeResolvers.Insert(0, new TypeResolver());

            var reader = new StringReader(specString);
            var deserializedSpec = deserializer.Deserialize<ConfigSpec>(reader);
        }

        class TypeResolver : INodeTypeResolver
        {
            #region Fields

            static Dictionary<string, Type> knownTypes = new Dictionary<string, Type>
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

            public bool Resolve(NodeEvent nodeEvent, ref Type currentType)
            {
                Type optionType;
                if (nodeEvent.Tag != null && knownTypes.TryGetValue(nodeEvent.Tag, out optionType))
                {
                    currentType = optionType;
                    return true;
                }

                return false;
            }

            #endregion
        }
    }
}
