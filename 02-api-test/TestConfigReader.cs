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

namespace apitest
{
    [TestFixture]
    public class TestConfigReader
    {
		ConfigReader reader;
		ConfigValidatorEventLogger validationEventLogger;

        [TestFixtureSetUp]
        public void Init()
        {
			reader = new ConfigReader();
			validationEventLogger = new ConfigValidatorEventLogger(Console.Out);
        }

        [Test]
        public void TestRelaxedMode()
        {
            Config config;
            var loadSuccess = reader.TryLoadFromFile("Examples\\ValidConfiguration.ini", out config, null, ConfigValidationMode.Relaxed, Encoding.UTF8);

            Assert.IsTrue(loadSuccess);
			Assert.IsTrue(config.IsValid(ConfigValidationMode.Relaxed, validationEventLogger));
        }

		[Test]
		public void TestStrictMode()
		{
			// TODO:
		}

		// TODO: test the main exceptions, reading and validation errors
    }
}
