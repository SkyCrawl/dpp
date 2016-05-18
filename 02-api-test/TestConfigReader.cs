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

namespace apitest
{
    [TestFixture]
    public class TestConfigReader
    {
        ConfigReader configReader;
        SpecReader specReader;

        IConfigValidatorEventLogger configValidationLogger;

        [OneTimeSetUp]
        public void Init()
        {
            configValidationLogger = Substitute.For<IConfigValidatorEventLogger>();

            var specValidationLogger = Substitute.For<ISpecValidatorEventLogger>();
            var configReaderLogger = Substitute.For<IConfigReaderEventLogger>();
            configReaderLogger.SpecValidatiorLogger.Returns(specValidationLogger);

            var specReaderLogger = Substitute.For<ISpecReaderEventLogger>();

            configReader = new ConfigReader(null, configReaderLogger);
            specReader = new SpecReader(specReaderLogger);
        }

        [Test]
        public void TestStrictMode()
        {
            Config config;
            var spec = specReader.LoadFromFile(Files.YamlSpec);
            var loadSuccess = configReader.TryLoadFromFile(Files.StrictConfig, out config, spec, ConfigValidationMode.Strict, Encoding.UTF8);

            Assert.IsTrue(loadSuccess);
            Assert.IsTrue(config.IsValid(ConfigValidationMode.Strict, configValidationLogger));
        }

        [Test]
        public void TestRelaxedMode()
        {
        }

        // TODO: test the main exceptions, reading and validation errors
    }
}
