using System;
using NUnit.Framework;
using YamlDotNet.Serialization;
using System.IO;
using Ini.Schema.Elements;

namespace ApiTest
{
	[TestFixture()]
	public class TestYAML
	{
        [Test()]
        public void TestYAMLDeserialization()
        {
            //var spec = new ConfigSpec("Test origin");

            //var section = new SectionSpec { Identifier = "Section 1", Description = "Section commentary", IsMandatory = true };
            //spec.Sections.Add(section);

            //var stringOption = new StringOptionSpec { Identifier = "String option", Description = "Option commentary", HasSingleValue = true, IsMandatory = true };
            //section.Options.Add(stringOption);

            //var enumOption = new EnumOptionSpec { Identifier = "Enum option", HasSingleValue = false, IsMandatory = false };
            //enumOption.AllowedValues.Add("Value1");
            //enumOption.AllowedValues.Add("Value2");
            //enumOption.DefaultValues.Add("Value1");
            //enumOption.DefaultValues.Add("Value1");
            //section.Options.Add(enumOption);

            //var serializer = new YamlDotNet.Serialization.Serializer(SerializationOptions.EmitDefaults | SerializationOptions.Roundtrip);
            //var writer = new StringWriter();
            //serializer.Serialize(writer, spec);

            //var specString = writer.ToString();

            var file = File.OpenText("YamlExample.txt");
            var specString = file.ReadToEnd();
                        
            var deserializer = new YamlDotNet.Serialization.Deserializer();
            deserializer.TypeResolvers.Insert(0, new TypeResolver());

            var reader = new StringReader(specString);
            var deserializedSpec = deserializer.Deserialize<ConfigSpec>(reader);
        }
	}

    class TypeResolver : INodeTypeResolver
    {
        #region INodeTypeResolver Members

        public bool Resolve(YamlDotNet.Core.Events.NodeEvent nodeEvent, ref Type currentType)
        {
            switch(nodeEvent.Tag)
            {
                case "!String":
                    currentType = typeof(StringOptionSpec);
                    return true;
                case "!Enum":
                    currentType = typeof(EnumOptionSpec);
                    return true;
            }

            return false;
        }

        #endregion
    }
}
