using System;
using System.IO;
using System.Reflection;

namespace Ini.Test
{
    internal static class Files
    {
        static internal readonly string YamlSpec = Path.Combine(AssemblyDirectory, "Examples", "Specification.yml");
        static internal readonly string InvalidYamlSpec = Path.Combine(AssemblyDirectory, "Examples", "InvalidSpecification.yml");

        static internal readonly string StrictConfig = Path.Combine(AssemblyDirectory, "Examples", "StrictConfiguration.ini");
        static internal readonly string RelaxedConfig = Path.Combine(AssemblyDirectory, "Examples", "RelaxedConfiguration.ini");
        static internal readonly string DefaultConfig = Path.Combine(AssemblyDirectory, "Examples", "DefaultConfiguration.ini");

        static internal readonly string SortedConfig = Path.Combine(AssemblyDirectory, "Examples", "SortedConfiguration.ini");
        static internal readonly string UnsortedConfig = Path.Combine(AssemblyDirectory, "Examples", "UnsortedConfiguration.ini");

        static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
