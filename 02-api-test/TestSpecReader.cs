using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini;
using Ini.EventLoggers;
using NUnit.Framework;

namespace apitest
{
    [TestFixture]
    public class TestSpecReader
    {
        ISpecReaderEventLogger readerLogger;
        ISpecValidatorEventLogger validationLogger;
		SpecReader reader;

        [TestFixtureSetUp]
        public void Init()
        {
            readerLogger = new SpecReaderEventLogger(Console.Out);
            validationLogger = new SpecValidatorEventLogger(Console.Out);
            reader = new SpecReader(readerLogger);
        }

        [Test()]
        public void TestDeserialization()
        {
            var spec = reader.LoadFromFile("Examples\\config.yml");

            Assert.IsTrue(spec.IsValid(validationLogger));
        }

		// TODO: test reading and validation errors
    }
}
