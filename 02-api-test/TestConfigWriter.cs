using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini;
using Ini.EventLoggers;
using NUnit.Framework;
using Ini.Configuration;
using Ini.Util;
using System.IO;

namespace Ini.Test
{
    [TestFixture]
    public class TestConfigWriter
    {
        ConfigReader reader;
        ConfigWriter writer;

        [TestFixtureSetUp]
        public void Init()
        {
            reader = new ConfigReader();
            writer = new ConfigWriter();
        }

        [Test]
        public void TestChainedIO()
        {
            // load the first config
            Config firstConfig;
            var loadSuccess = reader.TryLoadFromFile(Files.StrictConfig, out firstConfig, null, ConfigValidationMode.Relaxed, Encoding.UTF8);
            Assert.IsTrue(loadSuccess);

            // serialize it
            TextWriter serialized = new StringWriter();
            writer.WriteToText(serialized, firstConfig, null);

            // try to deserialize back
            Config secondConfig;
            loadSuccess = reader.TryLoadFromFile(Files.StrictConfig, out secondConfig, null, ConfigValidationMode.Relaxed, Encoding.UTF8);
            Assert.IsTrue(loadSuccess);

            // and test that they're equal
            Assert.IsTrue(firstConfig.Equals(secondConfig));
        }

        [Test]
        public void TestDefaultConfig()
        {
            // TODO:
        }
    }
}
