using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini;
using Ini.EventLoggers;
using NSubstitute;
using NUnit.Framework;

namespace Ini.Test
{
    [TestFixture]
    public class TestSpecReader
    {
        ISpecReaderEventLogger specReaderLogger;
        ISpecValidatorEventLogger specValidationLogger;

        SpecReader reader;

		[TestFixtureSetUp]
        public void Init()
        {
            specReaderLogger = Substitute.For<ISpecReaderEventLogger>();
            specValidationLogger = Substitute.For<ISpecValidatorEventLogger>();

            reader = new SpecReader(specReaderLogger);
        }

        [Test]
        public void TestDeserialization()
        {
            var spec = reader.LoadFromFile(Files.YamlSpec);

            specReaderLogger.Received().NewSpecification(Files.YamlSpec);
            Assert.IsTrue(spec.IsValid(specValidationLogger));
        }

        // TODO: test reading and validation errors
    }
}
