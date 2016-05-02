using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Ini;
using Ini.Specification;
using Ini.Specification.Values;

namespace apitest
{
    [TestFixture]
    public class TestSpec
    {
        [Test()]
        public void TestSpecSerialization()
        {
			var reader = new SchemaReader();
			reader.LoadFromFile("Examples\\config.yml");
        }
    }
}
