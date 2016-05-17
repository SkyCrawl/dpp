using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini;
using Ini.EventLoggers;
using NUnit.Framework;

namespace apitest.IntegrationTests
{
    [TestFixture]
    public class TestSpecification
    {
        SpecReader reader;
        ISpecReaderEventLogger readerLogger;
        ISpecValidatorEventLogger validationLogger;

        [TestFixtureSetUp]
        public void Init()
        {
            readerLogger = new SpecReaderEventLogger(Console.Out);
            validationLogger = new SpecValidatorEventLogger(Console.Out);

            reader = new SpecReader(readerLogger);
        }

        [Test()]
        public void TestSpecDeserialization()
        {
            var spec = reader.LoadFromFile("Examples\\config.yml");

            Assert.IsTrue(spec.IsValid(validationLogger));
        }

        // TODO: Test all reading and validation errors
    }
}
