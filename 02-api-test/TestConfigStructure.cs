using System;
using Ini.Configuration;
using NUnit.Framework;
using Ini.Exceptions;
using System.Collections.Generic;
using Ini.Configuration.Base;
using Ini.Configuration.Values;
using System.Linq;

namespace apitest
{
	[TestFixture]
	public class TestConfigStructure
	{
		/*
		 * HELPFUL LINKS:
		 * - http://www.nunit.org/index.php?p=attributes&r=2.6.4
		 * - http://www.nunit.org/index.php?p=constraintModel&r=2.6.4
		 */

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
			config.Items.Add(new KeyValuePair<string, ConfigBlockBase>("incorrect-identifier", new Commentary(null)));
		}

		[Test, ExpectedException(typeof(InvariantBrokenException))]
		public void TestConfigTypeBinding()
		{
			config.Items.Add("option", new Option("option", typeof(bool)));
		}

		[Test, ExpectedException(typeof(InvariantBrokenException))]
		public void TestSectionIdentifierMapping()
		{
			section.Items.Add(new KeyValuePair<string, ConfigBlockBase>("incorrect-identifier", new Commentary(null)));
		}

		[Test, ExpectedException(typeof(InvariantBrokenException))]
		public void TestSectionTypeBinding()
		{
			section.Items.Add(section.Identifier, section);
		}

		[Test, ExpectedException(typeof(InvariantBrokenException))]
		public void TestOptionTypeBinding()
		{
			option.Elements.Add(new StringValue("The internal observer should throw an exception because we're trying to add a value of a different type."));
		}

		[Test]
		[TestCase(TestName = "TestConversionToSingleValue (success)")]
		[TestCase(TestName = "TestConversionToSingleValue (failure)", ExpectedException = typeof(InvalidOperationException))]
		public void TestSingleElementConversion()
		{
			option.Elements.Add(new BoolValue(true));
			option.GetObjectValues().Single<IValue>();
		}

		[Test]
		public void TestBoolValueWithTrue([Values("1", "t", "y", "on", "yes", "enabled")] string valueToTest)
		{
			BoolValue value = new BoolValue(false);
			value.FillFromString(valueToTest);
			Assert.IsTrue(value.Value && valueToTest.Equals(value.ToOutputString(null)));
		}

		[Test]
		public void TestBoolValueWithFalse([Values("0", "f", "n", "off", "no", "disabled")] string valueToTest)
		{
			BoolValue value = new BoolValue(true);
			value.FillFromString(valueToTest);
			Assert.IsTrue(!value.Value && valueToTest.Equals(value.ToOutputString(null)));
		}

		[Test]
		public void TestLongValueIO([Values("0x123", "0123", "0b0101")] string valueToTest)
		{
			LongValue value = new LongValue(0);
			value.FillFromString(valueToTest);
			Assert.IsTrue(valueToTest.Equals(value.ToOutputString(null)));
		}

		[Test]
		public void TestULongValueIO([Values("0x123", "0123", "0b0101")] string valueToTest)
		{
			ULongValue value = new ULongValue(0);
			value.FillFromString(valueToTest);
			Assert.IsTrue(valueToTest.Equals(value.ToOutputString(null)));
		}

		[Test]
		public void TestNumberBase()
		{
			LongValue value = new LongValue(0); // but this test concerns unsigned value as well
			value.FillFromString("0x123");
			Assert.IsTrue(value.Base == Ini.Util.NumberBase.HEXADECIMAL);
			value.FillFromString("0123");
			Assert.IsTrue(value.Base == Ini.Util.NumberBase.OCTAL);
			value.FillFromString("0b0101");
			Assert.IsTrue(value.Base == Ini.Util.NumberBase.BINARY);
		}
	}
}
