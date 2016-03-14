using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace dpp_du_test
{
    enum ProgramToTest
    {
        BLOATED,
        CLEAN
    };

    [TestFixture()]
    public class Test
    {
        private static string TEST_DIR = new DirectoryInfo(GetCurrentDirectory()).Parent.Parent.FullName;
        private static string EMPTY_FILE = Path.Combine(TEST_DIR, "empty_file");
        private static string ONE_BYTE_FILE = Path.Combine(TEST_DIR, "one_byte_file");
        private static string INCOMPLETE_BYTE_SET_FILE = Path.Combine(TEST_DIR, "incomplete_byte_set_file");
        private static string COMPLETE_BYTE_SET_FILE = Path.Combine(TEST_DIR, "complete_byte_set_file");

        private static bool PRINT_PROGRAM_OUTPUT = false;

        [Test()]
        public void TestEmptyFile()
        {
            SameOutputForFile(new string[] { EMPTY_FILE });
        }

        [Test()]
        public void TestOneByteFile()
        {
            SameOutputForFile(new string[] { ONE_BYTE_FILE });
        }

        [Test()]
        public void TestIncompleteBytesetFile()
        {
            SameOutputForFile(new string[] { INCOMPLETE_BYTE_SET_FILE });
        }

        [Test()]
        public void TestCompleteBytesetFile()
        {
            SameOutputForFile(new string[] { COMPLETE_BYTE_SET_FILE });
        }

        [Test()]
        public void TestNoFile()
        {
            SameOutputForFile(new string[0]);
        }

        [Test()]
        public void TestNonExistentFile()
        {
            SameOutputForFile(new string[] { "asdefasdfasfd" });
        }

        private void SameOutputForFile(string[] args)
        {
            // save the default output stream
            var stdout = Console.Out;

            // run the programs with a copy of the arguments
            string result1 = ConsoleOutputOf(ProgramToTest.BLOATED, args.ToArray());
            string result2 = ConsoleOutputOf(ProgramToTest.CLEAN, args.ToArray());

            // print the two results, if required
            if (PRINT_PROGRAM_OUTPUT)
            {
                // first restore the default output stream
                Console.SetOut(stdout);

                // and then print
                Console.Out.WriteLine(string.Format("Bloated ({0}):", result1.Length));
                Console.Out.WriteLine(result1);
                Console.Out.WriteLine(string.Format("Clean ({0}):", result2.Length));
                Console.Out.WriteLine(result2);
            }

            // and check that they're the same
            Assert.True(result1.Equals(result2));
        }

        private String ConsoleOutputOf(ProgramToTest program, string[] args)
        {
            using (StringWriter sw = new StringWriter())
            {
                // the program's output will be written to our stream for inspection
                Console.SetOut(sw);

                // execute the program
                switch (program)
                {
                    case ProgramToTest.BLOATED:
                        HuffmanskeKapky.Program.MainOriginal(args);
                        break;
                    case ProgramToTest.CLEAN:
                        HuffmanCoding.Program.Main(args);
                        break;
                    default:
                        throw new System.InvalidOperationException();
                }

                // collect and return the result
                return sw.ToString();
            }
        }

        /// <summary>
        /// Returns the path to this assembly at the execution time.
        /// </summary>
        /// <returns></returns>
        static string GetCurrentDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            // CodeBase is in URI format
            var uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}
