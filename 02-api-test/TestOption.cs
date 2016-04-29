using System;
using NUnit.Framework;
using Ini.Configuration;
using Ini.Exceptions;
using Ini.Configuration.Values;

namespace ApiTest
{
	/*
	 * HELPFUL LINKS:
	 * - http://www.nunit.org/index.php?p=attributes&r=2.6.4
	 * - http://www.nunit.org/index.php?p=constraintModel&r=2.6.4
	 */

	[TestFixture]
	public class TestOption
	{
		private Option option;

		[TestFixtureSetUp]
		public void Init()
		{
			option = new Option("some-identifier", typeof(bool));
		}

		[Test, ExpectedException(typeof(InvariantBrokenException))]
		public void TestInternalObserver()
		{
			option.Values.Add(new StringValue("The internal observer should throw an exception because we're trying to add an element of a different type."));
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void TestSingleElementConversionOnEmptyOption()
		{
			option.GetSingleValue<bool>();
		}

		[Test]
		[TestCase(TestName = "TestSingleElementConversion (success)")]
		[TestCase(TestName = "TestSingleElementConversion (failure)", ExpectedException = typeof(InvalidOperationException))]
		public void TestSingleElementConversion()
		{
			option.Values.Add(new BoolValue(true));
			option.GetSingleValue<bool>();
		}
	}
}
