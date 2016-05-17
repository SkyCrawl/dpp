using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini;
using Ini.Configuration;
using Ini.EventLoggers;
using Ini.Util;
using NUnit.Framework;

namespace apitest.IntegrationTests
{
    [TestFixture]
    public class TestReader
    {
        IConfigValidatorEventLogger validatorLogger;

        [TestFixtureSetUp]
        public void Init()
        {
            validatorLogger = new ConfigValidatorEventLogger(Console.Out);
        }

        [Test]
        public void TestNoSpec()
        {
            Config config;
            var reader = new ConfigReader();
            var loadSuccess = reader.TryLoadFromFile("Examples\\ValidConfiguration.ini", out config, null, ConfigValidationMode.Relaxed, Encoding.UTF8);

            Assert.IsTrue(loadSuccess);
            Assert.IsTrue(config.IsValid(ConfigValidationMode.Relaxed, validatorLogger));
        }
    }
}
