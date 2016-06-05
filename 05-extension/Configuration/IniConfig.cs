using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Configuration.Base;

namespace Configuration.IniFile
{
    /// <summary>
    /// Input/output for file access.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FileIOConfig<T> : IDisposable
    {
        /// <summary>
        /// Path to file.
        /// </summary>
        protected readonly string Path;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">Path to file with object.</param>
        public FileIOConfig(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            Path = path;
        }
        
        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Load T from selected file.
        /// </summary>
        /// <returns>Loaded T.</returns>
        public T Load()
        {
            using (var reader = new StackReader(new StreamReader(Path), Path))
            {
                return Load(reader);
            }
        }

        /// <summary>
        /// Save T to selected T into selected file.
        /// </summary>
        /// <param name="obj">T for save.</param>
        public void Save(T obj)
        {
            using (var writer = new StreamWriter(Path))
            {
                Save(obj, writer);
            }
        }

        /// <summary>
        /// Load config from input value. Can be used for debug.
        /// </summary>
        /// <param name="inputValue">Input value with config.</param>
        /// <returns>Loaded config.</returns>
        internal T Load(string inputValue)
        {
            using (var reader = new StackReader(new StringReader(inputValue), Path))
            {
                return Load(reader);
            }
        }

        /// <summary>
        /// Save object to output. Can be used for debug.
        /// </summary>
        /// <param name="obj">Object for save.</param>
        /// <param name="output">Output for write.</param>
        internal void Save(T obj, StringBuilder output)
        {
            using (var writer = new StringWriter(output))
            {
                Save(obj, writer);
            }
        }
        
        /// <summary>
        /// Load <code>T</code> from input. Can be used for debug.
        /// </summary>
        /// <param name="reader">Input for load.</param>
        /// <returns>Loaded <code>T</code>.</returns>
        internal abstract T Load(StackReader reader);

        /// <summary>
        /// Save object to output. Can be used for debug.
        /// </summary>
        /// <param name="obj">Object for save.</param>
        /// <param name="writer">Output for write <code>T</code>.</param>
        internal abstract void Save(T obj, TextWriter writer);
    }

    /// <summary>
    /// Input/output of INI configuration.
    /// </summary>
    public class IniIOConfig : FileIOConfig<IConfig>, IIOConfig
    {
        public bool IsStrict
        {
            get
            {
                return _isStrict;
            }
        }

        const string INCLUDE_KEYWORD = "#include";

        private readonly IConfigDefinition _definition;
        private readonly bool _isStrict;

        const string ID_REGEX = @"[a-zA-Z.$:][a-zA-Z0-9_~-.:$ ]*";
        private Regex _regSection = null;
        private Regex _regValue = null;

        /// <summary>
        /// Relaxed constructor.
        /// </summary>
        /// <param name="path">Path to configuration file.</param>
        public IniIOConfig(string path)
            : base(path)
        {
            _definition = null;
            _isStrict = false;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">Path to configuration file.</param>
        /// <param name="definition">Definition of configuration. If this is strict implementation, definition can not be null.</param>
        /// <param name="isStrict">Whether this is strict loader implementation.</param>
        public IniIOConfig(string path, IConfigDefinition definition, bool isStrict)
            : base(path)
        {
            if (definition == null && isStrict)
            {
                throw new ArgumentNullException("definition");
            }
            _definition = definition;
            _isStrict = false;
        }

        /// <summary>
        /// Constructor only for debug methods.
        /// </summary>
        /// <param name="definition">Definition of configuration.</param>
        /// <param name="isStrict">Whether this is strict loader implementation.</param>
        internal IniIOConfig(IConfigDefinition definition, bool isStrict)
            : this(string.Empty, definition, isStrict)
        {
        }

        internal override IConfig Load(StackReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            LoadRegex();

            if (IsStrict)
            {
                return StrictLoad(reader);
            }
            return RelaxedLoad(reader);
        }

        internal override void Save(IConfig obj, TextWriter writer)
        {
            foreach (var item in obj)
            {
                Save(item, writer);
            }
        }

        private IConfig StrictLoad(StackReader reader)
        {
            var config = new Config(_definition);
            
            ISection section = null;

            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                if (IsInlude(line))
                {
                    HandleInclude(line, reader);
                    continue;
                }
                
                var item = ParseLoadLine(line, section);

                if (item == null)
                {
                    continue;
                }

                if (item is ISection)
                {
                    section = item as ISection;

                    if (config.Sections.Any(s => s.ID == section.ID))
                    {
                        throw new ConfigException("Section duplicated.");
                    }

                    config.Add(section);
                }
                else if (section == null)
                {
                    if (item is IComment)
                    {
                        config.Add(item);
                    }
                    else
                    {
                        throw new ConfigException(item, "Unexcepted type of item out of section.");
                    }
                }
                else
                {
                    if (item is IComment)
                    {
                        section.Add(item);
                    }
                    else if (item is IValueItem)
                    {
                        section.Add(item);
                    }
                    else
                    {
                        throw new ConfigException(item, "Unexcepted type of item in section.");
                    }
                }
            }

            if (!config.Check())
            {
                throw new ConfigException("All items of configuration is not valid.");
            }

            return config;
        }

        private void HandleInclude(string line, StackReader reader)
        {
            var path = line.Substring(INCLUDE_KEYWORD.Length).Trim();

            try
            {
                var basePath = System.IO.Path.GetDirectoryName(reader.CurrentPath);
                path = System.IO.Path.Combine(basePath, path);

                var textReader = new StreamReader(path);
                reader.PushReader(textReader, path);
            }
            catch(Exception ex)
            {
                throw new ConfigException("Invalid include path: {0}.", path);
            }
        }

        private bool IsInlude(string line)
        {
            if (line == null)
                return false;

            return line.StartsWith(INCLUDE_KEYWORD);
        }

        private IConfig RelaxedLoad(StackReader reader)
        {
            var config = new Config(_definition);
            
            ISection section = null;

            while (true)
            {
                var line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }

                if (line.Length == 0)
                {
                    continue;
                }

                var item = ParseLoadLine(line, section);

                if (item == null)
                {
                    continue;
                }

                if (item is ISection)
                {
                    section = item as ISection;
                    var existsSection = config.Sections.FirstOrDefault(s => s.ID == section.ID);
                    if (existsSection != null)
                    {
                        section = existsSection;
                    }
                    else
                    {
                        config.Add(item);
                    }
                }
                else if (section == null)
                {
                    config.Add(item);
                }
                else
                {
                    section.Add(item);
                }
            }

            return config;
        }

        private IConfigItem ParseLoadLine(string line, ISection currentSection)
        {
            line = line.TrimStart();

            if (line.Length == 0)
            {
                return null;
            }

            if (line[0] == ';')
            {
                return new CommentConfig(line.Substring(1).Trim());
            }

            if (_regSection.IsMatch(line))
            {
                var id = _regSection.Replace(line, "$1");
                var comment = _regSection.Replace(line, "$2");
                return CreateSection(id, comment);
            }

            if (_regValue.IsMatch(line))
            {
                var id = _regSection.Replace(line, "$1");
                var value = _regSection.Replace(line, "$2");
                var comment = _regSection.Replace(line, "$3");
                return CreateValueItem(id, value, comment, currentSection == null ? null : currentSection.Definition);
            }

            if (IsStrict)
            {
                throw new ConfigException("Unexcepted line - '{0}'", line);
            }

            return new OtherItem(line);
        }
        
        private ISection CreateSection(string id, string comment)
        {
            Section section;

            var secDefinition = _definition[id];
            if (secDefinition != null)
            {
                section = new Section(secDefinition);
            }
            else
            {
                if (IsStrict)
                {
                    throw new ConfigException("Not found definition for section.");
                }
                section = new Section(id);
            }

            if (!string.IsNullOrEmpty(comment))
            {
                section.SetComment(new CommentConfig(comment));
            }

            return section;
        }

        private IValueItem CreateValueItem(string id, string value, string comment, ISectionDefinition sectionDefinition)
        {
            if (IsStrict && sectionDefinition == null)
            {
                throw new ConfigException("In strict mode is required definition for value item - Id='{0}', Value='{1}'", id, value);
            }

            IValueItem item = null;

            if (sectionDefinition != null)
            {
                var valueDefinition = sectionDefinition[id];
                
                if ((valueDefinition == null || !valueDefinition.TryParse(value, out item)) && IsStrict)
                {
                    throw new ConfigException("Value item is not converted - ID='{0}', Value='{1}'", id, value);
                }
            }

            if (item == null)
            {
                var defaultItem = new StringValueItem(id);
                if (!string.IsNullOrEmpty(value))
                {
                    defaultItem.SetValue(value);
                }
                item = defaultItem;
            }

            if (!string.IsNullOrEmpty(comment))
            {
                item.SetComment(new CommentConfig(comment));
            }

            return item;
        }
        
        private void Save(IConfigItem item, TextWriter writer)
        {
            if (item == null)
            {
                return;
            }
            if (item is ISection)
            {
                Save(item as ISection, writer);
            }
            else if (item is IValueItem)
            {
                Save(item as IValueItem, writer);
            }
            else if (item is IComment)
            {
                Save(item as IComment, writer);
            }
            else if (item is IOtherItem)
            {
                Save(item as IOtherItem, writer);
            }
            else
            {
                throw new ConfigException(item, "Unknown type of item for save.");
            }
        }

        private void Save(IComment item, TextWriter writer)
        {
            writer.WriteLine("; {0}", item.Text);
        }

        private void Save(IValueItem item, TextWriter writer)
        {
            var line = string.Format("{0} = {1}", item.ID, item.HasValue ? item.GetStringValue() : string.Empty);
            line = line.Trim();
            if (item.Comment == null || item.Comment.IsEmpty)
            {
                writer.WriteLine(line);
            }
            else
            {
                writer.WriteLine("{0}\t; {1}", line, item.Comment.Text);
            }
        }

        private void Save(ISection item, TextWriter writer)
        {
            if (item.Comment == null || item.Comment.IsEmpty)
            {
                writer.WriteLine("[{0}]", item.GetStringValue());
            }
            else
            {
                writer.WriteLine("[{0}]\t; {1}", item.GetStringValue(), item.Comment.Text);
            }
            foreach (var valueItem in item)
            {
                Save(valueItem, writer);
            }
        }

        private void Save(IOtherItem item, TextWriter writer)
        {
            writer.WriteLine(item.GetStringValue());
        }

        private void LoadRegex()
        {
            if (_regSection == null)
            {
                _regSection = new Regex(@"^ *\[(" + ID_REGEX + @")\] *(;.*)? *$");
            }
            if (_regValue == null)
            {
                _regValue = new Regex(@"^ *" + ID_REGEX + @" *= *([^;]*)(;.*)? *$");
            }
        }
    }
    
    /// <summary>
    /// Input/output for configuration definition.
    /// </summary>
    public class IOConfigDefinition : FileIOConfig<IConfigDefinition>, IIOConfigDefinition
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path">Path to definition file.</param>
        public IOConfigDefinition(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Constructor only for debug methods.
        /// </summary>
        internal IOConfigDefinition()
            : base(string.Empty)
        {
        }

        internal override IConfigDefinition Load(StackReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            IConfigDefinition definition = new ConfigDefinition();
            ISectionDefinition section = null;

            while (true)
            {
                var line = reader.ReadLine();

                if (line == null)
                {
                    break;
                }

                if (line.Length == 0)
                {
                    continue;
                }

                switch (line)
                {
                    case "SECTION":
                        section = LoadSection(reader, definition);
                        break;

                    case "VALUE":
                        if (section == null)
                        {
                            throw new ConfigException("Section definition is not defined yet - Line='{0}'", line);
                        }
                        LoadValue(reader, section);
                        break;

                    default:
                        throw new ConfigException("Unexcepted definition line - '{0}'", line);
                }
            }

            return definition;
        }

        internal override void Save(IConfigDefinition obj, TextWriter writer)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            foreach (var section in obj)
            {
                Save(section, writer);
            }
        }

        private ISectionDefinition LoadSection(StackReader reader, IConfigDefinition definition)
        {
            var id = reader.ReadLine();
            var isRequired = IsRequired(reader);

            string line;
            do
            {
                line = reader.ReadLine();
            }
            while (!string.IsNullOrEmpty(line));

            var section = new SectionDefinition(id, isRequired);
            definition.Add(section);
            return section;
        }

        private void LoadValue(StackReader reader, ISectionDefinition definition)
        {
            var id = reader.ReadLine();
            var isRequired = IsRequired(reader);
            var typeId = reader.ReadLine();
            var isSingleValue = IsSingleValue(reader);

            var parameters = new List<string>();

            string line;
            do
            {
                line = reader.ReadLine();
                parameters.Add(line);
            }
            while (!string.IsNullOrEmpty(line));

            parameters.RemoveAt(parameters.Count - 1);

            ValueDefinition valueDefinition;
            if (!ValueDefinition.TryParse(typeId, id, isRequired, isSingleValue, parameters, out valueDefinition))
            {
                throw new ConfigException("Value definition was not parsed - ID='{0}'", id);
            }

            definition.Add(valueDefinition);
        }

        private bool IsRequired(StackReader reader)
        {
            var line = reader.ReadLine();
            switch (line)
            {
                case "REQUIRED": return true;
                case "NOT_REQUIRED": return false;
            }
            throw new ConfigException("Required required/not_required value - '{0}'", line);
        }

        private bool IsSingleValue(StackReader reader)
        {
            var line = reader.ReadLine();
            switch (line)
            {
                case "SINGLE_VALUE": return true;
                case "MULTI_VALUE": return false;
            }
            throw new ConfigException("Required single_value/multi_value value - '{0}'", line);
        }
        
        private void Save(ISectionDefinition section, TextWriter writer)
        {
            writer.WriteLine("SECTION");
            writer.WriteLine(section.ID);
            writer.WriteLine(section.IsRequired ? "REQUIRED" : "NOT_REQUIRED");
            writer.WriteLine();

            foreach (var valueDef in section.ValueDefinitions)
            {
                Save(valueDef, writer);
            }
        }
        
        private void Save(IValueDefinition valueDef, TextWriter writer)
        {
            writer.WriteLine("VALUE");
            writer.WriteLine(valueDef.ID);
            writer.WriteLine(valueDef.IsRequired ? "REQUIRED" : "NOT_REQUIRED");
            writer.WriteLine(valueDef.TypeID);
            writer.WriteLine(valueDef.IsSingleValue ? "SINGLE_VALUE" : "MULTI_VALUE");

            var parameters = valueDef.GetParameters();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    writer.WriteLine(parameter);
                }
            }

            writer.WriteLine();
        }
    }
}
