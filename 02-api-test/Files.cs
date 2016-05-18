using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ini.Test
{
    internal static class Files
    {
		static internal readonly string YamlSpec = Path.Combine(AssemblyDirectory, "Examples" + Path.DirectorySeparatorChar + "Specification.yml");
		static internal readonly string StrictConfig = Path.Combine(AssemblyDirectory, "Examples" + Path.DirectorySeparatorChar + "StrictConfiguration.ini");
		static internal readonly string RelaxedConfig = Path.Combine(AssemblyDirectory, "Examples" + Path.DirectorySeparatorChar + "RelaxedConfiguration.ini");

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
