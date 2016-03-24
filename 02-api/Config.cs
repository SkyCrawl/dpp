using System;
using System.Collections.Generic;
using System.Text;

namespace IniConfig
{
	public class OptionValue
	{
		// TODO:
		public List<string> components { get; private set; }
	}

	/// <summary>
	/// Option name to value mapping. One option may be defined several times in a given section.
	/// </summary>
	public class OptionMap
	{
		public Dictionary<string, List<OptionValue>> options { get; private set; }

		public OptionMap()
		{
			this.options = new Dictionary<string, List<OptionValue>>();
		}

		public void AddOption(string name, OptionValue value)
		{
			if(this.options.ContainsKey(name))
			{
				this.options[name].Add(value);
			}
			else
			{
				this.options.Add(name, new List<OptionValue>() { value } );
			}
		}
	}

	/// <summary>
	/// Section wrapper.
	/// </summary>
	public class Section
	{
		// each section has a constant identifier
		public string identifier { get; private set; }

		// each section has an option map
		public OptionMap optionMap { get; private set; }

		public Section(string identifier)
		{
			this.identifier = identifier;
			this.optionMap = new OptionMap();
		}
	}

	// options defined outside of a section (specification doesn't say anything about this)
	public class DefaultSection : Section
	{
		public static string DEFAULT_SECTION_IDENTIFIER = "IniConfigSuperDefaultSectionIdentifier";

		public DefaultSection() : base(DEFAULT_SECTION_IDENTIFIER)
		{
		}
	}

	public class Config
	{
		// the encoding used to read the input config
		public Encoding encoding { get; private set; }

		// partial section definitions are not allowed => Dictionary
		public Dictionary<string, Section> sections { get; private set; }

		public Section defaultSection
		{
			get
			{
				return sections[DefaultSection.DEFAULT_SECTION_IDENTIFIER];
			}
		}

		public Config(Encoding Encoding)
		{
			this.encoding = Encoding;
			this.sections = new Dictionary<string, Section>();
			this.Add(new DefaultSection());
		}

		public void Add(Section section)
		{
			if(sections.ContainsKey(section.identifier))
			{
				throw new DuplicateSectionException();
			}
			else
			{
				sections.Add(section.identifier, section);
			}
		}
	}
}
	