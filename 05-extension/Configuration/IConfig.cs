using System;
using System.Collections.Generic;

namespace Configuration
{
    /// <summary>
    /// Definition of configuration.
    /// </summary>
    public interface IConfigDefinition : IEnumerable<ISectionDefinition>
    {
        /// <summary>
        /// Section definition.
        /// </summary>
        /// <param name="id">Section identificator.</param>
        /// <returns>Section definition with selected ID.</returns>
        ISectionDefinition this[string id] { get; set; }

        /// <summary>
        /// Add section definition into configuration.
        /// </summary>
        /// <param name="definition">Section definition for insert.</param>
        void Add(ISectionDefinition definition);

        /// <summary>
        /// Remove section from configuration.
        /// </summary>
        /// <param name="id">ID of section.</param>
        void Remove(string id);
    }

    /// <summary>
    /// Definition of section in configuration.
    /// </summary>
    public interface ISectionDefinition : IEnumerable<IValueDefinition>
    {
        /// <summary>
        /// Section identificator.
        /// </summary>
        string ID { get; }
        /// <summary>
        /// Whether is section required.
        /// </summary>
        bool IsRequired { get; }
        /// <summary>
        /// Inserted value definitions.
        /// </summary>
        IEnumerable<IValueDefinition> ValueDefinitions { get; }

        /// <summary>
        /// Value definition.
        /// </summary>
        /// <param name="id">Identificator of value definition.</param>
        /// <returns>Value defition with selected ID.</returns>
        IValueDefinition this[string id] { get; set; }

        /// <summary>
        /// Add value definition into configuration.
        /// </summary>
        /// <param name="definition">Value definition for insert.</param>
        void Add(IValueDefinition definition);

        /// <summary>
        /// Remove value definition.
        /// </summary>
        /// <param name="id">Value definition identificator.</param>
        void Remove(string id);
    }

    /// <summary>
    /// Definition of value in configuration.
    /// </summary>
    public interface IValueDefinition
    {
        /// <summary>
        /// Identificator.
        /// </summary>
        string ID { get; }
        /// <summary>
        /// Type identificator.
        /// </summary>
        string TypeID { get; }
        /// <summary>
        /// Whether is value required.
        /// </summary>
        bool IsRequired { get; }
        /// <summary>
        /// Whether is single value.
        /// </summary>
        bool IsSingleValue { get; }

        /// <summary>
        /// Try parse value to new config item.
        /// </summary>
        /// <param name="value">Value for parse.</param>
        /// <param name="item">Created value or null.</param>
        /// <returns>Whether value was parsed.</returns>
        bool TryParse(string value, out IValueItem item);
        /// <summary>
        /// Returns other parameters for definition.
        /// </summary>
        /// <returns>String value parameters.</returns>
        IEnumerable<string> GetParameters();
    }

    /// <summary>
    /// Definition of value in configuration with selected type of value.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public interface IValueDefinition<T> : IValueDefinition
    {
        /// <summary>
        /// Default value of this item config.
        /// </summary>
        T DefaultValue { get; }

        /// <summary>
        /// Convert inserted value to string by definition parameters.
        /// </summary>
        /// <param name="value">Value for converse.</param>
        /// <returns>Converted value.</returns>
        string ConvertValueToString(T value);
    }

    /// <summary>
    /// Instance of configuration.
    /// </summary>
    public interface IConfig : IEnumerable<IConfigItem>
    {
        /// <summary>
        /// Config definition for this instance.
        /// </summary>
        IConfigDefinition Definition { get; }
        /// <summary>
        /// Comment for this configuration.
        /// </summary>
        IComment Comment { get; }
        /// <summary>
        /// Sections in configuration.
        /// </summary>
        IEnumerable<ISection> Sections { get; }

        /// <summary>
        /// Section implementation.
        /// </summary>
        /// <param name="sectionId">Section identificator.</param>
        /// <returns>Section with selected ID.</returns>
        ISection this[string sectionId] { get; set; }
        /// <summary>
        /// Item of this configuration.
        /// </summary>
        /// <param name="index">Index of item.</param>
        /// <returns>Item at selected position.</returns>
        IConfigItem this[int index] { get; }
        
        /// <summary>
        /// Found index of inserted item.
        /// </summary>
        /// <param name="item">Item for search.</param>
        /// <returns>Returns index of item if exists, otherwise -1.</returns>
        int IndexOf(IConfigItem item);

        /// <summary>
        /// Add item into configuration.
        /// </summary>
        /// <param name="item">Item for add.</param>
        void Add(IConfigItem item);
        /// <summary>
        /// Insert item into configuration at selected position.
        /// </summary>
        /// <param name="index">Selected position.</param>
        /// <param name="item">Item for insert.</param>
        void Insert(int index, IConfigItem item);
        /// <summary>
        /// Remove selected item.
        /// </summary>
        /// <param name="item">Item for remove.</param>
        /// <returns></returns>
        bool Remove(IConfigItem item);
        /// <summary>
        /// Remove item at selected position.
        /// </summary>
        /// <param name="index">Selected position.</param>
        void RemoveAt(int index);

        /// <summary>
        /// Check whether all items are valid.
        /// </summary>
        /// <returns>TRUE if all items are valid, otherwise FALSE.</returns>
        bool Check();
    }

    /// <summary>
    /// Type of item in configuration.
    /// </summary>
    public enum ConfigItemType
    {
        /// <summary>
        /// Configuration section
        /// </summary>
        Section,
        /// <summary>
        /// Value implementation
        /// </summary>
        Value,
        /// <summary>
        /// Comment
        /// </summary>
        Comment,
        /// <summary>
        /// Unknown item of configuration
        /// </summary>
        Other
    }

    /// <summary>
    /// Item of configuration instance.
    /// </summary>
    public interface IConfigItem
    {
        /// <summary>
        /// Type of item.
        /// </summary>
        ConfigItemType Type { get; }
        /// <summary>
        /// Wheter is item in valid status.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Returns string value of this item.
        /// </summary>
        /// <returns>String value of this item.</returns>
        string GetStringValue();
    }

    /// <summary>
    /// Instance of configuration section.
    /// </summary>
    public interface ISection : IConfigItem, IEnumerable<IConfigItem>
    {
        /// <summary>
        /// Value.
        /// </summary>
        /// <param name="valueId">Value identificator.</param>
        /// <returns>Value with selected identificator.</returns>
        IValueItem this[string valueId] { get; set; }

        /// <summary>
        /// Section identificator.
        /// </summary>
        string ID { get; }
        /// <summary>
        /// Comment for this section.
        /// </summary>
        IComment Comment { get; }
        /// <summary>
        /// Value items this section.
        /// </summary>
        IEnumerable<IValueItem> Values { get; }
        /// <summary>
        /// Section definition.
        /// </summary>
        ISectionDefinition Definition { get; }

        /// <summary>
        /// Index of selected item.
        /// </summary>
        /// <param name="item">Item for search.</param>
        /// <returns>Index of item if exists, otherwise -1.</returns>
        int IndexOf(IConfigItem item);

        /// <summary>
        /// Add item into section.
        /// </summary>
        /// <param name="item">Item for add.</param>
        void Add(IConfigItem item);
        /// <summary>
        /// Insert item into section at selected position.
        /// </summary>
        /// <param name="index">Index of position.</param>
        /// <param name="item">Item for add.</param>
        void Insert(int index, IConfigItem item);
        /// <summary>
        /// Remove item from section.
        /// </summary>
        /// <param name="item">Item for remove.</param>
        /// <returns>Returns TRUE if item was removed, otherwise FALSE.</returns>
        bool Remove(IConfigItem item);
        /// <summary>
        /// Remove item at selected position.
        /// </summary>
        /// <param name="index">Index of position for remove.</param>
        void RemoveAt(int index);

        /// <summary>
        /// Set comment.
        /// </summary>
        /// <param name="comment">Comment for set.</param>
        void SetComment(IComment comment);
    }

    /// <summary>
    /// Instance of value in configuration.
    /// </summary>
    public interface IValueItem : IConfigItem
    {
        /// <summary>
        /// Value identificator.
        /// </summary>
        string ID { get; }
        /// <summary>
        /// Definition of this item.
        /// </summary>
        IValueDefinition Definition { get; }
        /// <summary>
        /// Comment for this item.
        /// </summary>
        IComment Comment { get; }
        /// <summary>
        /// Whether item has defined value.
        /// </summary>
        bool HasValue { get; }
        
        /// <summary>
        /// Remove value from item.
        /// </summary>
        void RemoveValue();
        
        /// <summary>
        /// Set comment.
        /// </summary>
        /// <param name="comment">Comment for set.</param>
        void SetComment(IComment comment);
    }

    /// <summary>
    /// Configuration comment.
    /// </summary>
    public interface IComment : IConfigItem
    {
        /// <summary>
        /// Whether is comment empty.
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// Whether is comment read only.
        /// </summary>
        bool IsReadOnly { get; }
        /// <summary>
        /// Text this comment.
        /// </summary>
        string Text { get; set; }
    }
    
    /// <summary>
    /// Unknown item of configuration.
    /// </summary>
    public interface IOtherItem : IConfigItem
    {
    }

    /// <summary>
    /// Input/output of configuration.
    /// </summary>
    public interface IIOConfig : IDisposable
    {
        /// <summary>
        /// Whether this is strict loader.
        /// </summary>
        bool IsStrict { get; }

        /// <summary>
        /// Load configuration.
        /// </summary>
        /// <returns>Configuration.</returns>
        IConfig Load();
        /// <summary>
        /// Save configuration.
        /// </summary>
        /// <param name="config">Configuration for save.</param>
        void Save(IConfig config);
    }

    /// <summary>
    /// Input/output of definition of configuration.
    /// </summary>
    public interface IIOConfigDefinition : IDisposable
    {
        /// <summary>
        /// Load definition.
        /// </summary>
        /// <returns>Definition.</returns>
        IConfigDefinition Load();
        /// <summary>
        /// Save definition.
        /// </summary>
        /// <param name="config">Definition for save.</param>
        void Save(IConfigDefinition config);
    }

    /// <summary>
    /// Reference to value item.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public interface IReferenceValue<T>
    {
        /// <summary>
        /// Is value at reference valid?
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Value at reference.
        /// </summary>
        T Value { get; }
    }
}