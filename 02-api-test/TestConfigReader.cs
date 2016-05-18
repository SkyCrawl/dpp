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
        ConfigReader configReader;
        SpecReader specReader;

        IConfigValidatorEventLogger configValidationLogger;

        [TestFixtureSetUp]
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
            config = configReader.LoadFromFile(Files.StrictConfig, spec, ConfigValidationMode.Strict, Encoding.UTF8);
            var loadSuccess = configReader.TryLoadFromFile(Files.StrictConfig, out config, spec, ConfigValidationMode.Strict, Encoding.UTF8);

            Assert.IsTrue(loadSuccess);
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
