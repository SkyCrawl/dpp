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
		Config config;
		Section section;
		Option option;

		[OneTimeSetUp]
		public void Init()
		{
			config = new Config();
			section = new Section("section");
			option = new Option("option", typeof(bool));
		}

		[Test]
		public void TestConfigIdentifierMapping()
		{
			Assert.Throws(typeof(InvariantBrokenException), () => 
				config.Items.Add(new KeyValuePair<string, ConfigBlockBase>("incorrect-identifier", new Commentary(null))));
		}

		[Test]
		public void TestConfigTypeBinding()
		{
			Assert.Throws(typeof(InvariantBrokenException), () => 
				config.Items.Add("option", new Option("option", typeof(bool))));
		}

		[Test]
		public void TestSectionIdentifierMapping()
		{
			Assert.Throws(
				typeof(InvariantBrokenException),
				() => 
				section.Items.Add(new KeyValuePair<string, ConfigBlockBase>(
					"incorrect-identifier",
					new Commentary(null))));
		}

		[Test]
		public void TestSectionTypeBinding()
		{
			Assert.Throws(
				typeof(InvariantBrokenException),
				() => 
				section.Items.Add(section.Identifier, section));
		}

		[Test]
		public void TestOptionTypeBinding()
		{
			Assert.Throws(
				typeof(InvariantBrokenException),
				() => 
				option.Elements.Add(new StringValue("The internal observer should throw an exception because we're trying to add a value of a different type.")));
		}

		[Test, Order(1)]
		public void TestSingleElementConversionSuccess()
		{
			option.Elements.Add(new BoolValue(true));
			option.GetObjectValues().Single<IValue>();
		}

		[Test, Order(2)]
		public void TestSingleElementConversionFailure()
		{
			Assert.Throws(
				typeof(InvalidOperationException),
				() =>
				{
					option.Elements.Add(new BoolValue(true));
					option.GetObjectValues().Single<IValue>();
				});
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
