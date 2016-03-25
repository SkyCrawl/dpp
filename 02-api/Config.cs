using System;
using System.Collections.Generic;
using System.Text;

namespace IniConfig
{
	/// <summary>
	/// Typed element representing the whole value of an option, or a part of it.
	/// </summary>
	public class StringComponent : ElementComponent<string>
	{
		// TODO: element typu string může obsahovat libovolné znaky s výjimkou ',', ':' a ';', kde je třeba je uvést znakem '\'

		public StringComponent(string value) : base(value, value)
		{
		}
	}

	/// <summary>
	/// Typed element representing the whole value of an option, or a part of it.
	/// </summary>
	public class EnumComponent : ElementComponent<Enum>
	{
		// TODO: element typu enum může nabývat hodnot z předem definované množiny řetězců

		public EnumComponent(string original, Enum resolved) : base(original, resolved)
		{
		}
	}

	/// <summary>
	/// Typed element representing the whole value of an option, or a part of it.
	/// </summary>
	public class FloatComponent : ElementComponent<float>
	{
		// TODO: element typu float může nabývat hodnot v rozsahu určeném 64-bitovou reprezentací čísel v plovoucí řádové čárce (IEEE 754), zapsaných v obvyklém formátu

		public FloatComponent(string original, float resolved) : base(original, resolved)
		{
		}
	}

	/// <summary>
	/// Typed element representing the whole value of an option, or a part of it.
	/// </summary>
	public class UIntComponent : ElementComponent<uint>
	{
		// TODO: element typu unsigned může nabývat hodnot v rozsahu určeném 64-bitovou reprezentací přirozených čísel
		// TODO: elementy typu signed a unsigned specifikují čísla v desítkové soustavě; reprezentace v šestnáctkové, osmičkové, nebo dvojkové soustavě je nutno uvést s prefixem '0x', '0', respektive '0b'

		public UIntComponent(string original, uint resolved) : base(original, resolved)
		{
		}
	}

	/// <summary>
	/// Typed element representing the whole value of an option, or a part of it.
	/// </summary>
	public class IntComponent : ElementComponent<int>
	{
		// TODO: element typu signed může nabývat hodnot v rozsahu určeném 64-bitovou reprezentací celých čísel ve dvojkovém doplňku
		// TODO: elementy typu signed a unsigned specifikují čísla v desítkové soustavě; reprezentace v šestnáctkové, osmičkové, nebo dvojkové soustavě je nutno uvést s prefixem '0x', '0', respektive '0b'

		public IntComponent(string original, int resolved) : base(original, resolved)
		{
		}
	}

	/// <summary>
	/// Typed element representing the whole value of an option, or a part of it.
	/// </summary>
	public class BoolComponent : ElementComponent<bool>
	{
		// TODO: element typu boolean může nabývat hodnot z množiny { 0, f, n, off, no, disabled } pro ohodnocení false nebo hodnot z množiny { 1, t, y, on, yes, enabled } pro ohodnocení true

		public BoolComponent(string original, bool resolved) : base(original, resolved)
		{
		}
	}

	public interface IElementComponent
	{
		X asComponent<X>();
	}

	public abstract class ElementComponent<T> : IElementComponent
	{
		protected string original;
		protected T resolved;

		public ElementComponent(string original, T resolved)
		{
			this.original = original;
			this.resolved = resolved;
		}

		public X asComponent<X>()
		{
			Type thisType = this.GetType();
			Type thatType = typeof(X);
			if(thatType.IsSubclassOf(thisType) && (typeof(T) == thatType.GetGenericArguments()[0]))
			{
				return (X) (IElementComponent) this;
			}
			else
			{
				// TODO: more elaborate
				throw new System.InvalidCastException();
			}
		}
	}

	/// <summary>
	/// Abstract element representing the whole value of an option, or a part of it.
	/// </summary>
	public class Element
	{
		// TODO:
		public List<IElementComponent> values { get; private set; }

		/*
		 * TODO - type checks:
		 * - text elementu může obsahovat odkaz na hodnotu volby v nějaké skupině
		 * 		- formát odkazu je ${sekce#volba}
		 * 		- hodnota se vkládá jako textová náhrada, t.j. případné speciální znaky a příkazy se musí po vložení interpretovat
		 * 		- odkaz se neinterpretuje pokud znaku $ předchází znak '\'
		 */
	}

	/// <summary>
	/// All options defined in a section.
	/// </summary>
	public class Options
	{
		// specification doesn't say whether duplicate option identifiers are allowed so let's map identifiers to lists
		public Dictionary<string, List<Element>> options { get; private set; }

		public Options()
		{
			this.options = new Dictionary<string, List<Element>>();
		}

		public void AddOption(string name, Element value)
		{
			if(this.options.ContainsKey(name))
			{
				this.options[name].Add(value);
			}
			else
			{
				this.options.Add(name, new List<Element>() { value } );
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
		public Options optionMap { get; private set; }

		public Section(string identifier)
		{
			this.identifier = identifier;
			this.optionMap = new Options();
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
	