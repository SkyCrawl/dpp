using System;
using NUnit.Framework;
using Ini.Configuration;
using Ini.Util.Guid;
using Ini.Configuration.Values;
using Ini.Configuration.Values.Links;
using Ini.Util.LinkResolving;
using Ini.Exceptions;
using Ini.EventLoggers;
using System.Linq;
using Ini.Configuration.Base;

namespace apitest
{
	[TestFixture]
	public class TestLinks
	{
		Config config;
		Section section;

		[TestFixtureSetUp]
		public void Init()
		{
			config = new Config();
			section = new Section("section");
			config.Add(section);
		}

		[Test, ExpectedException(typeof(LinkCycleException))]
		public void TestLinkCycleDetected()
		{
			section.Clear();

			Option option1 = new Option("option1", typeof(bool));
			Option option2 = new Option("option2", typeof(bool));
			Option option3 = new Option("option3", typeof(bool));

			InclusionLink link1 = new InclusionLink(typeof(bool), new LinkTarget(section.Identifier, "option2"));
			InclusionLink link2 = new InclusionLink(typeof(bool), new LinkTarget(section.Identifier, "option3"));
			InclusionLink link3 = new InclusionLink(typeof(bool), new LinkTarget(section.Identifier, "option1"));

			option1.Elements.Add(link1);
			option2.Elements.Add(link2);
			option3.Elements.Add(link3);

			section.Add(option1);
			section.Add(option2);
			section.Add(option3);

			LinkResolver resolver = new LinkResolver();
			resolver.AddLink(new LinkNode(link1, new LinkOrigin(section.Identifier, option1.Identifier)));
			resolver.AddLink(new LinkNode(link2, new LinkOrigin(section.Identifier, option2.Identifier)));
			resolver.AddLink(new LinkNode(link3, new LinkOrigin(section.Identifier, option3.Identifier)));
			resolver.ResolveLinks(config, new ConfigReaderEventLogger(Console.Out));
		}

		[Test]
		public void TestLinkResolvingLogic()
		{
			section.Clear();

			Option option1 = new Option("option1", typeof(bool));
			Option option2 = new Option("option2", typeof(bool));
			Option option3 = new Option("option3", typeof(bool));

			BoolValue value1 = new BoolValue(true);
			InclusionLink link2 = new InclusionLink(typeof(bool), new LinkTarget(section.Identifier, "option1"));
			InclusionLink link3 = new InclusionLink(typeof(bool), new LinkTarget(section.Identifier, "option2"));

			option1.Elements.Add(value1);
			option2.Elements.Add(link2);
			option3.Elements.Add(link3);

			section.Add(option1);
			section.Add(option2);
			section.Add(option3);

			LinkResolver resolver = new LinkResolver();
			resolver.AddLink(new LinkNode(link2, new LinkOrigin(section.Identifier, option1.Identifier)));
			resolver.AddLink(new LinkNode(link3, new LinkOrigin(section.Identifier, option2.Identifier)));
			resolver.ResolveLinks(config, new ConfigReaderEventLogger(Console.Out));

			Assert.IsTrue(link3.IsResolved);
			link3.InterpretSelf();
			IValue result = link3.Values.Single();
			Assert.IsTrue(result is BoolValue && (result as BoolValue).Value);
		}
	}
}
