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

        [SetUp]
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

        [Test]
        public void TestInvalidSpec()
        {
            var spec = reader.LoadFromFile(Files.InvalidYamlSpec);

            Assert.IsFalse(spec.IsValid(specValidationLogger));
            specValidationLogger.Received().DuplicateSection("Sekce 1");
            specValidationLogger.Received().DuplicateOption("Sekce 1", "Option 1");
            specValidationLogger.Received().NoValue("Other", "bool2");
            specValidationLogger.Received().TooManyValues("Other", "bool1");
            specValidationLogger.Received().NoEnumValues("Enums", "The Option");
            specValidationLogger.Received().ValueNotAllowed("Enums", "The Option 2", "Value4");
            specValidationLogger.Received().ValueOutOfRange("Cisla", "cele", -1L);
        }
    }
}
