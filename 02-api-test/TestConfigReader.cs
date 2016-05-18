using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini;
using Ini.Configuration;
using Ini.EventLoggers;
using Ini.Util;
using NSubstitute;
using NUnit.Framework;

namespace Ini.Test
{
    [TestFixture]
	public class TestConfigReader
    {
		SpecReader specReader;
		IConfigValidatorEventLogger configValidationLogger;
        ConfigReader configReader;

		[TestFixtureSetUp]
        public void Init()
        {
			var specReaderLogger = Substitute.For<ISpecReaderEventLogger>();
            var specValidationLogger = Substitute.For<ISpecValidatorEventLogger>();
            var configReaderLogger = Substitute.For<IConfigReaderEventLogger>();
			configReaderLogger.SpecValidatiorLogger.Returns(specValidationLogger);

			this.specReader = new SpecReader(specReaderLogger);
			this.configValidationLogger = Substitute.For<IConfigValidatorEventLogger>();
            this.configReader = new ConfigReader(null, configReaderLogger);
        }

        [Test]
        public void TestStrictMode()
        {
			/*
            Config config;
            var spec = specReader.LoadFromFile(Files.YamlSpec);
            var loadSuccess = configReader.TryLoadFromFile(Files.StrictConfig, out config, spec, ConfigValidationMode.Strict, Encoding.UTF8);

            Assert.IsTrue(loadSuccess);
            Assert.IsTrue(config.IsValid(ConfigValidationMode.Strict, configValidationLogger));
            */

			var spec = specReader.LoadFromFile(Files.YamlSpec);
			var config = configReader.LoadFromFile(Files.StrictConfig, spec, ConfigValidationMode.Strict, Encoding.UTF8);

			Assert.IsNotNull(config);
			Assert.IsTrue(config.IsValid(ConfigValidationMode.Strict, configValidationLogger));
        }

        [Test]
        public void TestRelaxedMode()
        {
			Config config = configReader.LoadFromFile(Files.RelaxedConfig, null, ConfigValidationMode.Relaxed, Encoding.UTF8);

			Assert.IsNotNull(config);

			// TODO: other assertations
        }
    }
}
