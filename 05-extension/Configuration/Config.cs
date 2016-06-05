using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Configuration.Base
{
    public class Config : IConfig
    {
        public IConfigItem this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        public ISection this[string sectionId]
        {
            get
            {
                ISection section;
                if (_sections.TryGetValue(sectionId, out section))
                {
                    return section;
                }
                return null;
            }

            set
            {
                _sections[sectionId] = value;
            }
        }

        public IComment Comment
        {
            get;
            set;
        }

        public IConfigDefinition Definition
        {
            get
            {
                return _definition;
            }
        }

        public IEnumerable<ISection> Sections
        {
            get
            {
                return _sections.Values;
            }
        }

        private readonly IConfigDefinition _definition;
        private List<IConfigItem> _items = new List<IConfigItem>();
        private Dictionary<string, ISection> _sections = new Dictionary<string, ISection>();
        
        public Config(IConfigDefinition definition)
        {
            _definition = definition;
        }

        public void Add(IConfigItem item)
        {
            _items.Add(item);
            var section = item as ISection;
            if (section != null && !_sections.ContainsKey(section.ID))
            {
                _sections.Add(section.ID, section);
            }
        }

        public bool Check()
        {
            if (_definition == null)
            {
                return true;
            }
            foreach (var defSection in _definition)
            {
                var section = this[defSection.ID];
                if (section == null)
                {
                    if (defSection.IsRequired)
                    {
                        return false;
                    }
                    continue;
                }
                foreach (var defValue in defSection)
                {
                    if (defValue.IsRequired && !section.Values.Any(v => v.ID == defValue.ID))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public IEnumerator<IConfigItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(IConfigItem item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, IConfigItem item)
        {
            _items.Insert(index, item);
        }

        public bool Remove(IConfigItem item)
        {
            if (_items.Remove(item))
            {
                var section = item as ISection;
                if (section != null)
                {
                    _sections.Remove(section.ID);
                }
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            var section = _items[index] as ISection;
            if (section != null)
            {
                _sections.Remove(section.ID);
            }
            _items.RemoveAt(index);
        }
    }

    public class Section : ISection
    {
        public IValueItem this[string valueId]
        {
            get
            {
                IValueItem item;
                if (_valueItems.TryGetValue(valueId, out item))
                {
                    return item;
                }
                return null;
            }

            set
            {
                _valueItems[valueId] = value;
            }
        }

        public IComment Comment
        {
            get;
            private set;
        }

        public string ID
        {
            get
            {
                if (Definition != null)
                {
                    return Definition.ID;
                }
                return _id;
            }
        }

        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        public ConfigItemType Type
        {
            get
            {
                return ConfigItemType.Section;
            }
        }

        public IEnumerable<IValueItem> Values
        {
            get
            {
                return _valueItems.Values;
            }
        }

        public ISectionDefinition Definition
        {
            get
            {
                return _definition;
            }
        }

        private readonly ISectionDefinition _definition = null;
        private readonly string _id = string.Empty;
        private List<IConfigItem> _items = new List<IConfigItem>();
        private Dictionary<string, IValueItem> _valueItems = new Dictionary<string, IValueItem>();

        public Section(ISectionDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }
            _definition = definition;
        }

        public Section(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Identificator can not be null or empty.", "id");
            }
            _id = id;
        }

        public void Add(IConfigItem item)
        {
            _items.Add(item);
            var valueItem = item as IValueItem;
            if (valueItem != null && !_valueItems.ContainsKey(valueItem.ID))
            {
                _valueItems.Add(valueItem.ID, valueItem);
            }
        }

        public IEnumerator<IConfigItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string GetStringValue()
        {
            return ID;
        }

        public int IndexOf(IConfigItem item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, IConfigItem item)
        {
            _items.Insert(index, item);
        }

        public bool Remove(IConfigItem item)
        {
            if (_items.Remove(item))
            {
                var valueItem = item as IValueItem;
                if (valueItem != null)
                {
                    _valueItems.Remove(valueItem.ID);
                }
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            var valueItem = _items[index] as IValueItem;
            if (valueItem != null)
            {
                _valueItems.Remove(valueItem.ID);
            }
            _items.RemoveAt(index);
        }

        public void SetComment(IComment comment)
        {
            if (comment == null)
            {
                comment = CommentConfig.Empty;
            }
            Comment = comment;
        }
    }

    /// <summary>
    /// Value item of configuration.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public class ValueItem<T> : IValueItem
    {
        public IComment Comment { get; private set; }
        public bool HasValue { get; private set; }

        public string ID
        {
            get
            {
                if (Definition != null)
                {
                    return Definition.ID;
                }
                return _id;
            }
        }
        public IValueDefinition Definition
        {
            get
            {
                return TypedDefinition;
            }
        }
        public bool IsValid
        {
            get
            {
                return HasValue || !Definition.IsRequired;
            }
        }
        public ConfigItemType Type
        {
            get
            {
                return ConfigItemType.Value;
            }
        }

        public readonly IValueDefinition<T> TypedDefinition;

        public T Value
        {
            get
            {
                return HasValue ? _value : TypedDefinition.DefaultValue;
            }
        }

        public bool IsRequired
        {
            get
            {
                return TypedDefinition.IsRequired;
            }
        }

        protected IReferenceValue<T> RefValue
        {
            get;
            private set;
        }

        private readonly string _id;
        private T _value;
        
        public ValueItem(IValueDefinition<T> definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }
            TypedDefinition = definition;
            Comment = CommentConfig.Empty;
            HasValue = false;
            _id = string.Empty;
            _value = default(T);
            RefValue = null;
        }
        
        public ValueItem(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Identificator can not be null or empty.", "id");
            }
            TypedDefinition = null;
            Comment = CommentConfig.Empty;
            HasValue = false;
            _id = id;
            _value = default(T);
            RefValue = null;
        }

        public void SetValue(T value)
        {
            HasValue = true;
            _value = value;
            RefValue = null;
        }

        public void SetValue(IReferenceValue<T> reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException("reference");
            }
            HasValue = true;
            _value = default(T);
            RefValue = reference;
        }

        public void RemoveValue()
        {
            HasValue = false;
            _value = default(T);
            RefValue = null;
        }

        public virtual string GetStringValue()
        {
            var value = RefValue != null ? RefValue.Value : Value;
            if (TypedDefinition != null)
            {
                return TypedDefinition.ConvertValueToString(value);
            }
            return value.ToString();
        }

        public void SetComment(IComment comment)
        {
            if (comment == null)
            {
                comment = CommentConfig.Empty;
            }
            Comment = comment;
        }
    }

    /// <summary>
    /// Bool value item of configuration.
    /// </summary>
    public sealed class BoolValueItem : ValueItem<bool>
    {
        public BoolValueItem(IValueDefinition<bool> definition)
            : base(definition)
        {
        }

        public BoolValueItem(string id)
            : base(id)
        {
        }

        public override string GetStringValue()
        {
            var value = RefValue != null ? RefValue.Value : Value;
            if (TypedDefinition != null)
            {
                return TypedDefinition.ConvertValueToString(value);
            }
            return value ? BoolValueDefinition.DEFAULT_TRUE : BoolValueDefinition.DEFAULT_FALSE;
        }
    }

    /// <summary>
    /// Int64 value item of configuration.
    /// </summary>
    public class Int64ValueItem : ValueItem<long>
    {
        public Int64ValueItem(IValueDefinition<long> definition)
            : base(definition)
        {
        }

        public Int64ValueItem(string id)
            : base(id)
        {
        }
    }
    
    /// <summary>
    /// Unsigned Int64 value item of configuration.
    /// </summary>
    public class UInt64ValueItem : ValueItem<ulong>
    {
        public UInt64ValueItem(IValueDefinition<ulong> definition)
            : base(definition)
        {
        }

        public UInt64ValueItem(string id)
            : base(id)
        {
        }
    }

    /// <summary>
    /// Double value item of configuration.
    /// </summary>
    public class DoubleValueItem : ValueItem<double>
    {
        public DoubleValueItem(IValueDefinition<double> definition)
            : base(definition)
        {
        }

        public DoubleValueItem(string id)
            : base(id)
        {
        }
    }

    /// <summary>
    /// Date value item of configuration.
    /// </summary>
    public class DateValueItem : ValueItem<DateTime>
    {
        public DateValueItem(IValueDefinition<DateTime> definition)
            : base(definition)
        {
        }

        public DateValueItem(string id)
            : base(id)
        {
        }
    }

    /// <summary>
    /// Local value item of configuration.
    /// </summary>
    public class LocaleValueItem : ValueItem<CultureInfo>
    {
        public LocaleValueItem(IValueDefinition<CultureInfo> definition)
            : base(definition)
        {
        }

        public LocaleValueItem(string id)
            : base(id)
        {
        }
    }

    /// <summary>
    /// Enum value item of configuration.
    /// </summary>
    public class EnumValueItem : ValueItem<string>
    {
        public EnumValueItem(IValueDefinition<string> definition)
            : base(definition)
        {
        }

        public EnumValueItem(string id)
            : base(id)
        {
        }

        public override string GetStringValue()
        {
            return Value;
        }
    }

    /// <summary>
    /// String value item of configuration.
    /// </summary>
    public class StringValueItem : ValueItem<string>
    {
        public StringValueItem(IValueDefinition<string> definition)
            : base(definition)
        {
        }

        public StringValueItem(string id)
            : base(id)
        {
        }

        public override string GetStringValue()
        {
            return Value;
        }
    }
    
    /// <summary>
    /// Item type for bad defined lines in files.
    /// </summary>
    public class OtherItem : IOtherItem
    {
        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        public ConfigItemType Type
        {
            get
            {
                return ConfigItemType.Other;
            }
        }

        public readonly string Value;

        public OtherItem(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            Value = value;
        }

        public string GetStringValue()
        {
            return Value;
        }
    }

    /// <summary>
    /// Comment.
    /// </summary>
    public class CommentConfig : IComment
    {
        /// <summary>
        /// Empty comment read-only implementation.
        /// </summary>
        public static CommentConfig Empty
        {
            get
            {
                if (_empty == null)
                {
                    _empty = new CommentConfig();
                    _empty.IsReadOnly = true;
                }
                return _empty;
            }
        }
        private static CommentConfig _empty = null;

        public CommentConfig(string text)
        {
            Text = text;
            IsReadOnly = false;
        }

        private CommentConfig()
            : this(string.Empty)
        {
        }

        public bool IsReadOnly { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(Text);
            }
        }

        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (IsReadOnly)
                {
                    throw new ApplicationException("Comment is readonly");
                }
                if (string.IsNullOrEmpty(value))
                {
                    _text = string.Empty;
                }
                else
                {
                    _text = value;
                }
            }
        }

        public ConfigItemType Type
        {
            get
            {
                return ConfigItemType.Comment;
            }
        }

        private string _text;

        public string GetStringValue()
        {
            return _text;
        }
    }

    /// <summary>
    /// Runtime configuration exception.
    /// </summary>
    public class ConfigException : Exception
    {
        public readonly IConfigItem ConfigItem;

        public ConfigException()
        {
            ConfigItem = null;
        }

        public ConfigException(string message, params object[] args)
            : base(string.Format(message, args))
        {
            ConfigItem = null;
        }
        
        public ConfigException(IConfigItem configItem, string message, params object[] args)
            : base(string.Format(message, args))
        {
            ConfigItem = configItem;
        }
    }

    public class ConfigDefinition : IConfigDefinition
    {
        public ISectionDefinition this[string id]
        {
            get
            {
                ISectionDefinition section;
                if (_sections.TryGetValue(id, out section))
                {
                    return section;
                }
                return null;
            }

            set
            {
                _sections[id] = value;
            }
        }

        private Dictionary<string, ISectionDefinition> _sections = new Dictionary<string, ISectionDefinition>();

        public ConfigDefinition()
        {
        }

        public void Add(ISectionDefinition definition)
        {
            _sections.Add(definition.ID, definition);
        }

        public void Remove(string id)
        {
            _sections.Remove(id);
        }

        public IEnumerator<ISectionDefinition> GetEnumerator()
        {
            return _sections.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class SectionDefinition : ISectionDefinition
    {
        public IValueDefinition this[string id]
        {
            get
            {
                IValueDefinition value;
                if (_valueDefinitions.TryGetValue(id, out value))
                {
                    return value;
                }
                return null;
            }

            set
            {
                _valueDefinitions[id] = value;
            }
        }

        public string ID
        {
            get
            {
                return _id;
            }
        }

        public bool IsRequired
        {
            get
            {
                return _isRequired;
            }
        }

        public IEnumerable<IValueDefinition> ValueDefinitions
        {
            get
            {
                return _valueDefinitions.Values;
            }
        }

        private readonly string _id;
        private readonly bool _isRequired;
        private Dictionary<string, IValueDefinition> _valueDefinitions = new Dictionary<string, IValueDefinition>();

        public SectionDefinition(string id, bool isRequired)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Identificator can not be null or empty.", "id");
            }
            _id = id;
            _isRequired = isRequired;
        }

        public void Add(IValueDefinition definition)
        {
            _valueDefinitions.Add(definition.ID, definition);
        }

        public void Remove(string id)
        {
            _valueDefinitions.Remove(id);
        }

        public IEnumerator<IValueDefinition> GetEnumerator()
        {
            return _valueDefinitions.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class ValueDefinition : IValueDefinition
    {
        public static bool TryParse(string typeId, string id, bool isRequired, bool isSingleValue, IList<string> parameters, out ValueDefinition valueDefinition)
        {
            switch (typeId)
            {
                case BoolValueDefinition.TYPE_ID:
                    valueDefinition = new BoolValueDefinition(id, parameters);
                    break;

                case Int64ValueDefinition.TYPE_ID:
                    valueDefinition = new Int64ValueDefinition(id, parameters);
                    break;

                case UInt64ValueDefinition.TYPE_ID:
                    valueDefinition = new UInt64ValueDefinition(id, parameters);
                    break;

                case DoubleValueDefinition.TYPE_ID:
                    valueDefinition = new DoubleValueDefinition(id, parameters);
                    break;

                case StringValueDefinition.TYPE_ID:
                    valueDefinition = new StringValueDefinition(id, parameters);
                    break;

                case EnumValueDefinition.TYPE_ID:
                    valueDefinition = new EnumValueDefinition(id, parameters);
                    break;

                case DateValueDefinition.TYPE_ID:
                    valueDefinition = new DateValueDefinition(id, parameters);
                    break;

                case LocaleValueDefinition.TYPE_ID:
                    valueDefinition = new LocaleValueDefinition(id, parameters);
                    break;

                default:
                    valueDefinition = null;
                    return false;
            }

            valueDefinition.IsRequired = isRequired;
            valueDefinition.IsSingleValue = isSingleValue;
            return true;
        }

        public string ID
        {
            get
            {
                return _id;
            }
        }

        public bool IsRequired
        {
            get;
            protected set;
        }

        public virtual bool IsSingleValue
        {
            get;
            protected set;
        }
        
        private readonly string _id;

        public ValueDefinition(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Identificator can not be null or empty.", "id");
            }
            _id = id;
            IsRequired = false;
            IsSingleValue = true;
        }

        public abstract string TypeID { get; }
        public abstract bool TryParse(string value, out IValueItem item);
        public abstract IEnumerable<string> GetParameters();
    }

    public class BoolValueDefinition : ValueDefinition, IValueDefinition<bool>
    {
        public const string TYPE_ID = "BOOL_VALUE";
        public const string DEFAULT_TRUE = "1";
        public const string DEFAULT_FALSE = "0";

        public override string TypeID
        {
            get
            {
                return TYPE_ID;
            }
        }

        public bool DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        private readonly bool _defaultValue;
        private readonly string _valueFalse;
        private readonly string _valueTrue;

        public BoolValueDefinition(string id, IList<string> parameters)
            : base(id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _defaultValue = (parameters.Count >= 1 && parameters[0] == "true") ? true : false;
            _valueTrue = parameters.Count >= 2 ? parameters[1] : DEFAULT_TRUE;
            _valueFalse = parameters.Count >= 3 ? parameters[2] : DEFAULT_FALSE;
        }

        public override bool TryParse(string value, out IValueItem item)
        {
            bool bValue;

            if (value == _valueTrue)
            {
                bValue = true;
            }
            else if (value == _valueFalse)
            {
                bValue = false;
            }
            else
            {
                switch (value)
                {
                    case "1":
                    case "t":
                    case "y":
                    case "on":
                    case "yes":
                    case "enabled":
                        bValue = true;
                        break;

                    case "0":
                    case "f":
                    case "n":
                    case "off":
                    case "no":
                    case "disabled":
                        bValue = false;
                        break;

                    default:
                        item = null;
                        return false;
                }
            }

            item = new BoolValueItem(this);
            (item as BoolValueItem).SetValue(bValue);
            return true;
        }

        public override IEnumerable<string> GetParameters()
        {
            return new string[] { _defaultValue ? "true" : "false", _valueTrue, _valueFalse };
        }

        public string ConvertValueToString(bool value)
        {
            return value ? _valueTrue : _valueFalse;
        }
    }

    public class Int64ValueDefinition : ValueDefinition, IValueDefinition<long>
    {
        public const string TYPE_ID = "INT64_VALUE";
        
        public override string TypeID
        {
            get
            {
                return TYPE_ID;
            }
        }

        public long DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        private readonly long _defaultValue;

        public Int64ValueDefinition(string id, IList<string> parameters)
            : base(id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _defaultValue = parameters.Count >= 1 ? long.Parse(parameters[0]) : default(long);
        }

        public override bool TryParse(string value, out IValueItem item)
        {
            long lValue;
            if (!long.TryParse(value, out lValue))
            {
                item = null;
                return false;
            }

            item = new Int64ValueItem(this);
            (item as Int64ValueItem).SetValue(lValue);
            return true;
        }

        public override IEnumerable<string> GetParameters()
        {
            return new string[] { _defaultValue.ToString() };
        }

        public string ConvertValueToString(long value)
        {
            return value.ToString();
        }
    }

    public class UInt64ValueDefinition : ValueDefinition, IValueDefinition<ulong>
    {
        public const string TYPE_ID = "UINT64_VALUE";

        public override string TypeID
        {
            get
            {
                return TYPE_ID;
            }
        }

        public ulong DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        private readonly ulong _defaultValue;

        public UInt64ValueDefinition(string id, IList<string> parameters)
            : base(id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _defaultValue = parameters.Count >= 1 ? ulong.Parse(parameters[0]) : default(ulong);
        }

        public override bool TryParse(string value, out IValueItem item)
        {
            ulong lValue;
            if (!ulong.TryParse(value, out lValue))
            {
                item = null;
                return false;
            }

            item = new UInt64ValueItem(this);
            (item as UInt64ValueItem).SetValue(lValue);
            return true;
        }

        public override IEnumerable<string> GetParameters()
        {
            return new string[] { _defaultValue.ToString() };
        }

        public string ConvertValueToString(ulong value)
        {
            return value.ToString();
        }
    }

    public class DoubleValueDefinition : ValueDefinition, IValueDefinition<double>
    {
        public const string TYPE_ID = "DOUBLE_VALUE";

        public override string TypeID
        {
            get
            {
                return TYPE_ID;
            }
        }

        public double DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        private readonly double _defaultValue;

        public DoubleValueDefinition(string id, IList<string> parameters)
            : base(id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _defaultValue = parameters.Count >= 1 ? double.Parse(parameters[0], CultureInfo.InvariantCulture) : default(double);
        }

        public override bool TryParse(string value, out IValueItem item)
        {
            double dValue;
            if (!double.TryParse(value, out dValue))
            {
                item = null;
                return false;
            }

            item = new DoubleValueItem(this);
            (item as DoubleValueItem).SetValue(dValue);
            return true;
        }

        public override IEnumerable<string> GetParameters()
        {
            return new string[] { _defaultValue.ToString(CultureInfo.InvariantCulture) };
        }

        public string ConvertValueToString(double value)
        {
            return value.ToString();
        }
    }

    public class DateValueDefinition : ValueDefinition, IValueDefinition<DateTime>
    {
        public const string TYPE_ID = "DATE_VALUE";

        public override string TypeID
        {
            get
            {
                return TYPE_ID;
            }
        }

        public DateTime DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        private readonly DateTime _defaultValue;

        public DateValueDefinition(string id, IList<string> parameters)
            : base(id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _defaultValue = parameters.Count >= 1 ? DateTime.Parse(parameters[0], CultureInfo.InvariantCulture) : default(DateTime);
        }

        public override bool TryParse(string value, out IValueItem item)
        {
            DateTime dValue;
            if (!DateTime.TryParse(value, out dValue))
            {
                item = null;
                return false;
            }

            item = new DateValueItem(this);
            (item as DateValueItem).SetValue(dValue);
            return true;
        }

        public override IEnumerable<string> GetParameters()
        {
            return new string[] { _defaultValue.ToString(CultureInfo.InvariantCulture) };
        }

        public string ConvertValueToString(DateTime value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public class LocaleValueDefinition : ValueDefinition, IValueDefinition<CultureInfo>
    {
        public const string TYPE_ID = "LOCALE_VALUE";

        public override string TypeID
        {
            get
            {
                return TYPE_ID;
            }
        }

        public CultureInfo DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        private readonly CultureInfo _defaultValue;

        public LocaleValueDefinition(string id, IList<string> parameters)
            : base(id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _defaultValue = parameters.Count >= 1 ? CultureInfo.GetCultureInfo(parameters[0]) : default(CultureInfo);
        }

        public override bool TryParse(string value, out IValueItem item)
        {
            try
            {
                var lValue = CultureInfo.GetCultureInfo(value);

                item = new LocaleValueItem(this);
                (item as LocaleValueItem).SetValue(lValue);

                return true;
            }
            catch(Exception)
            {
                item = null;
                return false;
            }
        }

        public override IEnumerable<string> GetParameters()
        {
            return new string[] { _defaultValue.Name };
        }

        public string ConvertValueToString(CultureInfo value)
        {
            return value.Name;
        }
    }

    public class StringValueDefinition : ValueDefinition, IValueDefinition<string>
    {
        public const string TYPE_ID = "STRING_VALUE";
        
        public override string TypeID
        {
            get
            {
                return TYPE_ID;
            }
        }

        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        private readonly string _defaultValue;

        public StringValueDefinition(string id, IList<string> parameters)
            : base(id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _defaultValue = parameters.Count >= 1 ? parameters[0] : string.Empty;
        }

        public override bool TryParse(string value, out IValueItem item)
        {
            item = new StringValueItem(this);
            value = value.Replace(@"\,", ",");
            value = value.Replace(@"\:", ":");
            value = value.Replace(@"\;", ";");
            (item as StringValueItem).SetValue(value);
            return true;
        }

        public override IEnumerable<string> GetParameters()
        {
            return new string[] { _defaultValue };
        }

        public string ConvertValueToString(string value)
        {
            value = value.Replace(",", @"\,");
            value = value.Replace(":", @"\:");
            value = value.Replace(";", @"\;");
            return value;
        }
    }
    
    public class EnumValueDefinition : ValueDefinition, IValueDefinition<string>
    {
        public const string TYPE_ID = "ENUM_VALUE";

        public override string TypeID
        {
            get
            {
                return TYPE_ID;
            }
        }

        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
        }

        private readonly string _defaultValue;
        private readonly List<string> _enumValues;

        public EnumValueDefinition(string id, IList<string> parameters)
            : base(id)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
            _defaultValue = parameters.Count >= 1 ? parameters[0] : string.Empty;
            _enumValues = new List<string>();
            foreach (var parameter in parameters)
            {
                _enumValues.Add(parameter);
            }
        }

        public override bool TryParse(string value, out IValueItem item)
        {
            if (!_enumValues.Contains(value, StringComparer.CurrentCultureIgnoreCase))
            {
                item = null;
                return false;
            }
            item = new EnumValueItem(this);
            (item as EnumValueItem).SetValue(value);
            return true;
        }

        public override IEnumerable<string> GetParameters()
        {
            return _enumValues;
        }

        public string ConvertValueToString(string value)
        {
            return value;
        }
    }
}