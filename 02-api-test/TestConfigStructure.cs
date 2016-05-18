using System;
using Ini.Configuration;
using NUnit.Framework;
using Ini.Exceptions;
using System.Collections.Generic;
using Ini.Configuration.Base;
using Ini.Configuration.Values;
using System.Linq;

namespace Ini.Test
{
	[TestFixture]
	public class TestConfigStructure
	{
		Config config;
		Section section;
		Option option;
		string[] longIntegersToTest;

		[TestFixtureSetUp]
		public void Init()
		{
			config = new Config();
			section = new Section("section");
			option = new Option("option", typeof(bool));
			longIntegersToTest = new string[] { "0x123", "0123", "0b101" };
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
			option.GetObjectValues().Single<IValue>();
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void TestSingleElementConversionFailure()
		{
			option.Elements.Clear();
			option.Elements.Add(new BoolValue(true));
			option.Elements.Add(new BoolValue(true));
			option.GetObjectValues().Single<IValue>();
		}

		[Test]
		public void TestBoolValueWithTrue()
		{
			foreach(string valueToTest in BoolValue.TrueStrings.Keys)
			{
				BoolValue value = new BoolValue(false);
				value.FillFromString(valueToTest);
				Assert.IsTrue(value.Value && valueToTest.Equals(value.ToOutputString(null)));
			}
		}

		[Test]
		public void TestBoolValueWithFalse()
		{
			foreach(string valueToTest in BoolValue.FalseStrings.Keys)
			{
				BoolValue value = new BoolValue(true);
				value.FillFromString(valueToTest);
				Assert.IsTrue(!value.Value && valueToTest.Equals(value.ToOutputString(null)));
			}
		}

		[Test]
		public void TestLongValueIO()
		{
			foreach(string valueToTest in longIntegersToTest)
			{
				LongValue value = new LongValue(0);
				value.FillFromString(valueToTest);
				Assert.IsTrue(valueToTest.Equals(value.ToOutputString(null)));
			}
		}

		[Test]
		public void TestULongValueIO()
		{
			foreach(string valueToTest in longIntegersToTest)
			{
				ULongValue value = new ULongValue(0);
				value.FillFromString(valueToTest);
				Assert.IsTrue(valueToTest.Equals(value.ToOutputString(null)));
			}
		}

		[Test]
		public void TestNumberBase()
		{
			LongValue value = new LongValue(0); // but this test concerns unsigned value as well
			value.FillFromString("0x123");
			Assert.IsTrue(value.Base == Ini.Util.NumberBase.HEXADECIMAL);
			value.FillFromString("0123");
			Assert.IsTrue(value.Base == Ini.Util.NumberBase.OCTAL);
			value.FillFromString("0b101");
			Assert.IsTrue(value.Base == Ini.Util.NumberBase.BINARY);
		}
	}
}
