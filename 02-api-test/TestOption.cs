using System;
using System.Linq;
using NUnit.Framework;
using Ini.Configuration;
using Ini.Exceptions;
using Ini.Configuration.Values;
using Ini.Configuration.Base;

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
			option.Elements.Add(new StringValue("The internal observer should throw an exception because we're trying to add a value of a different type."));
		}

		[Test]
		[TestCase(TestName = "TestSingleElementConversion (success)")]
		[TestCase(TestName = "TestSingleElementConversion (failure)", ExpectedException = typeof(InvalidOperationException))]
		public void TestSingleElementConversion()
		{
			option.Elements.Add(new BoolValue(true));
			option.GetObjectValues().Single<IValue>();
		}
	}
}
