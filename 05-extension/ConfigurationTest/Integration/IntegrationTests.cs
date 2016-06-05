using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Configuration;
using Configuration.Base;
using Configuration.IniFile;
using NUnit.Framework;
using System;

namespace ConfigurationTest.Integration
{
    [TestFixture]
    class IntegrationTests
    {
        const string DIR_PATH = @"..\..\Integration\TestData";
        const string EXTENSION = "ini";

        string TestFilePath(string testName)
        {
            return Path.Combine(AssemblyDirectory, DIR_PATH, string.Format("{0}.{1}", testName, EXTENSION));
        }
        
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

        [TestFixtureSetUp]
        public void IniciallizeTests()
        {

        }

        [Test]
        public void EmptyFileTest()
        {
            var dir = Assembly.GetExecutingAssembly().Location;


            var configFile = new IniIOConfig(TestFilePath("Emty"));

            IConfig config = configFile.Load();

            //var item = config["section 1"]["id1"];
            //if (item is DoubleValueItem)
            //{
            //    (item as DoubleValueItem).SetValue(5.0);
            //}

            //configFile.Save(config);
            Assert.IsTrue(true);
        }

        [Test]
        public void IncludeFileTest()
        {
            var configFile = new IniIOConfig(TestFilePath("Include"));

            IConfig config = configFile.Load();

            Assert.IsTrue(config.Sections.Any());
        }

        [TestFixtureTearDown]
        public void ClearTestData()
        {

        }
    }
}
