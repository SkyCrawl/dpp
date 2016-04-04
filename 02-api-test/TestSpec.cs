using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Ini;
using Ini.Specification;
using Ini.Specification.Elements;

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
			deserializer.TypeResolvers.Insert(0, new SchemaTypeResolver());

            var reader = new StringReader(specString);
            var deserializedSpec = deserializer.Deserialize<ConfigSpec>(reader);
        }
    }
}
