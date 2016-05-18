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
using NSubstitute;
using Ini.Specification;

namespace Ini.Test
{
    [TestFixture]
    public class TestConfigWriter
    {
		SpecReader specReader;
		ConfigReader configReader;
		ConfigWriter configWriter;

        [TestFixtureSetUp]
        public void Init()
        {
			var configReaderLogger = Substitute.For<IConfigReaderEventLogger>();
			configReaderLogger.SpecValidatiorLogger.Returns(Substitute.For<ISpecValidatorEventLogger>());

			this.specReader = new SpecReader(Substitute.For<ISpecReaderEventLogger>());
			this.configReader = new ConfigReader(null, configReaderLogger);
			this.configWriter = new ConfigWriter(Substitute.For<IConfigWriterEventLogger>());
        }

        [Test]
        public void TestChainedIOWithRelaxedMode()
        {
			// prepare writer options
			ConfigWriterOptions options = new ConfigWriterOptions();
			options.Validate = false;

            // load the first configuration from file
			Config config1 = configReader.LoadFromFile(Files.RelaxedConfig, null, ConfigValidationMode.Relaxed, Encoding.UTF8);

            // serialize it and backup the result
            TextWriter writer1 = new StringWriter();
			configWriter.WriteToText(writer1, config1, options);
			String serialized1 = writer1.ToString();

            // try to deserialize it back into the second configuration
			// TODO: this parses identifier with the trailing ']'
			Config config2 = configReader.LoadFromText(null, new StringReader(serialized1), null, ConfigValidationMode.Relaxed);

			// serialize it and backup the result
			TextWriter writer2 = new StringWriter();
			configWriter.WriteToText(writer2, config2, options);
			String serialized2 = writer2.ToString();

            // and test that the serialized configurations are identical
			Assert.IsTrue(serialized1 == serialized2);
        }

        [Test]
		public void TestChainedIOWithStrictMode()
        {
			// load the first configuration from file
			ConfigSpec specification = specReader.LoadFromFile(Files.YamlSpec);
			Config config1 = configReader.LoadFromFile(Files.StrictConfig, specification, ConfigValidationMode.Strict, Encoding.UTF8);

			// serialize it and backup the result
			TextWriter writer1 = new StringWriter();
			configWriter.WriteToText(writer1, config1, null);
			String serialized1 = writer1.ToString();

			// try to deserialize it back into the second configuration
			Config config2 = configReader.LoadFromText(null, new StringReader(serialized1), specification, ConfigValidationMode.Strict);

			// serialize it and backup the result
			TextWriter writer2 = new StringWriter();
			configWriter.WriteToText(writer2, config2, null);
			String serialized2 = writer2.ToString();

			// and test that the serialized configurations are identical
			Assert.IsTrue(serialized1 == serialized2);

			/*
			using (var stream = File.Open("/Users/SkyCrawl/Downloads/serialized2.txt", FileMode.Create, FileAccess.Write))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(serialized2);
				stream.Write(bytes, 0, bytes.Length);
			}
			*/
        }

		// TODO: TestDefaultConfig
    }
}
