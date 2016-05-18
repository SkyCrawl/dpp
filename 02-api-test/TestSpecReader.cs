using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ini;
using Ini.EventLoggers;
using Ini.Specification.Values;
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
        public void TestValidSpec()
        {
            var spec = reader.LoadFromFile(Files.YamlSpec);
            specReaderLogger.Received().NewSpecification(Files.YamlSpec);

            Assert.IsTrue(spec.IsValid(specValidationLogger));

            var section = spec.Sections[0];
            Assert.AreEqual(section.Identifier, "Sekce 1");
            Assert.AreEqual(section.Description, "the purpose of Sekce 1");
            Assert.IsTrue(section.IsMandatory);

            section = spec.Sections[4];
            Assert.IsFalse(section.IsMandatory);

            var option = spec.Sections[0].Options[0];
            Assert.IsInstanceOf<StringOptionSpec>(option);
            Assert.AreEqual(option.Identifier, "Option 1");
            Assert.AreEqual(option.Description, "volba 'Option 1' ma hodnotu 'value 1'");
            Assert.IsTrue(option.IsMandatory);
            Assert.IsTrue(option.HasSingleValue);

            option = spec.Sections[2].Options[0];
            Assert.IsInstanceOf<LongOptionSpec>(option);

            option = spec.Sections[2].Options[1];
            Assert.IsInstanceOf<ULongOptionSpec>(option);

            option = spec.Sections[2].Options[4];
            Assert.IsInstanceOf<DoubleOptionSpec>(option);

            option = spec.Sections[3].Options[0];
            Assert.IsInstanceOf<BooleanOptionSpec>(option);

            option = spec.Sections[4].Options[0];
            Assert.IsInstanceOf<EnumOptionSpec>(option);
            Assert.IsFalse(option.IsMandatory);
            Assert.IsFalse(option.HasSingleValue);

            var enumOption = (EnumOptionSpec)option;
            Assert.IsNotEmpty(enumOption.DefaultValues);
            Assert.AreEqual(enumOption.DefaultValues[0], "Value1");
            Assert.IsNotEmpty(enumOption.AllowedValues);
            Assert.AreEqual(enumOption.AllowedValues[0], "Value1");
            Assert.AreEqual(enumOption.AllowedValues[1], "Value2");
            Assert.AreEqual(enumOption.AllowedValues[2], "Value3");
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
