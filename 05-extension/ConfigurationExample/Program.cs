using Configuration;
using Configuration.Base;
using Configuration.IniFile;

namespace ConfigurationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            WithoutDefinitionExample("config.ini");
            RelaxExample("config.ini", "definition_file.def");
            StrictExample("config.ini", "definition_file.def");
        }

        static void WithoutDefinitionExample(string configFilePath)
        {
            var configFile = new IniIOConfig(configFilePath);

            IConfig config = configFile.Load();

            var item = config["section 1"]["id1"];
            if (item is DoubleValueItem)
            {
                (item as DoubleValueItem).SetValue(5.0);
            }

            configFile.Save(config);
        }

        static void RelaxExample(string configFilePath, string definitionFilePath)
        {
            var defFile = new IOConfigDefinition(definitionFilePath);

            IConfigDefinition definition = defFile.Load();

            var configFile = new IniIOConfig(configFilePath, definition, false);

            IConfig config = configFile.Load();

            var item = config["section 1"]["id1"];
            if (item is DoubleValueItem)
            {
                (item as DoubleValueItem).SetValue(5.0);
            }

            configFile.Save(config);
        }

        static void StrictExample(string configFilePath, string definitionFilePath)
        {
            var defFile = new IOConfigDefinition(definitionFilePath);

            IConfigDefinition definition = defFile.Load();

            var configFile = new IniIOConfig(configFilePath, definition, true);

            IConfig config = configFile.Load();

            var item = config["section 1"]["id1"];
            if (item is DoubleValueItem)
            {
                (item as DoubleValueItem).SetValue(5.0);
            }

            configFile.Save(config);
        }
    }
}
