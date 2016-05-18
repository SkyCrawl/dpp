using System;
using Ini.Configuration;
using NUnit.Framework;
using Ini.Exceptions;
using System.Collections.Generic;
using Ini.Configuration.Base;
using Ini.Configuration.Values;
using System.Linq;
using Ini.Util;

namespace Ini.Test
{
    [TestFixture]
    public class TestConfigStructure
    {
        static string[] trueStrings = new string[]
        {
            "1",
            "t",
            "y",
            "on",
            "yes",
            "enabled"
        };

        static string[] falseStrings = new string[]
        {
            "0",
            "f",
            "n",
            "off",
            "no",
            "disabled",
        };

        static string[] longIntegersToTest = new string[]
        {
            "0x123",
            "0123",
            "0b101"
        };

        Config config;
        Section section;
        Option option;

        [TestFixtureSetUp]
        public void Init()
        {
            config = new Config();
            section = new Section("section");
            option = new Option("option", typeof(bool));
        }

        [Test, ExpectedException(typeof(InvariantBrokenException))]
        public void TestConfigIdentifierMapping()
        {
            // internal observer should throw as we're trying to add an invalid key-value pair to a configuration
            config.Items.Add(new KeyValuePair<string, ConfigBlockBase>("incorrect-identifier", new Commentary(new string[] {})));
        }

        [Test, ExpectedException(typeof(InvariantBrokenException))]
        public void TestConfigTypeBinding()
        {
            // internal observer should throw as we're trying to add an option to a configuration
            config.Items.Add("option", new Option("option", typeof(bool)));
        }

        [Test, ExpectedException(typeof(InvariantBrokenException))]
        public void TestSectionIdentifierMapping()
        {
            // internal observer should throw as we're trying to add an invalid key-value pair to a section
            section.Items.Add(new KeyValuePair<string, ConfigBlockBase>(
                    "incorrect-identifier",
                new Commentary(new string[] {})));
        }

        [Test, ExpectedException(typeof(InvariantBrokenException))]
        public void TestSectionTypeBinding()
        {
            // internal observer should throw as we're trying to add a section inside a section
            section.Items.Add(section.Identifier, section);
        }

        [Test, ExpectedException(typeof(InvariantBrokenException))]
        public void TestOptionTypeBinding()
        {
            option.Elements.Add(new StringValue("The internal observer should throw an exception because we're trying to add a value of a different type."));
        }

        [Test]
        public void TestSingleElementConversionSuccess()
        {
            option.Elements.Clear();
            option.Elements.Add(new BoolValue(true));
            option.GetObjectValues().Single();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestSingleElementConversionFailure()
        {
            option.Elements.Clear();
            option.Elements.Add(new BoolValue(true));
            option.Elements.Add(new BoolValue(true));
            option.GetObjectValues().Single();
        }

        [Test]
        public void TestBoolValueWithTrue()
        {
            foreach (string valueToTest in trueStrings)
            {
                BoolValue value = new BoolValue(false);
                value.FillFromString(valueToTest);

                Assert.IsTrue(value.Value);
                Assert.AreEqual(valueToTest, value.ToOutputString(null));
            }
        }

        [Test]
        public void TestBoolValueWithFalse()
        {
            foreach(string valueToTest in falseStrings)
            {
                BoolValue value = new BoolValue(true);
                value.FillFromString(valueToTest);

                Assert.IsFalse(value.Value);
                Assert.AreEqual(valueToTest, value.ToOutputString(null));
            }
        }

        [Test]
        public void TestLongValueIO()
        {
            foreach(string valueToTest in longIntegersToTest)
            {
                LongValue value = new LongValue(0);
                value.FillFromString(valueToTest);

                Assert.AreEqual(valueToTest, value.ToOutputString(null));
            }
        }

        [Test]
        public void TestULongValueIO()
        {
            foreach(string valueToTest in longIntegersToTest)
            {
                ULongValue value = new ULongValue(0);
                value.FillFromString(valueToTest);

                Assert.AreEqual(valueToTest, value.ToOutputString(null));
            }
        }

        [Test]
        public void TestNumberBaseLong()
        {
            LongValue value = new LongValue(0);

            value.FillFromString("0x123");
            Assert.AreEqual(value.Base, NumberBase.HEXADECIMAL);

            value.FillFromString("0123");
            Assert.AreEqual(value.Base, NumberBase.OCTAL);

            value.FillFromString("0b101");
            Assert.AreEqual(value.Base, NumberBase.BINARY);
        }

        [Test]
        public void TestNumberBaseULong()
        {
            ULongValue value = new ULongValue(0);

            value.FillFromString("0x123");
            Assert.AreEqual(value.Base, NumberBase.HEXADECIMAL);

            value.FillFromString("0123");
            Assert.AreEqual(value.Base, NumberBase.OCTAL);

            value.FillFromString("0b101");
            Assert.AreEqual(value.Base, NumberBase.BINARY);
        }
    }
}
