using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuration.Base;

namespace Configuration
{
    internal class StackReader : IDisposable
    {
        #region Fields

        Stack<TextReader> readers = new Stack<TextReader>();
        Stack<string> paths = new Stack<string>();

        #endregion

        #region Properties

        /// <summary>
        /// Path for the current reader.
        /// </summary>
        public string CurrentPath
        {
            get
            {
                if (paths.Count == 0)
                    return null;

                return paths.Peek();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StackReader"/> class.
        /// </summary>
        /// <param name="reader">The initial reader.</param>
        /// <param name="path">The path of the initial reader.</param>
        public StackReader(TextReader reader, string path)
        {
            PushReader(reader, path);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads a new line from the last added reader.
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            string result = null;

            while(result == null)
            {
                // First check if there is a reader on the stack.
                if (readers.Count == 0)
                    break;

                var reader = readers.Peek();

                // Read line from the reader. If there are no more lines, then dispose reader and switch to next one.
                result = reader.ReadLine();
                if (result == null)
                {
                    reader.Dispose();
                    readers.Pop();
                    paths.Pop();
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a new reader on the stack.
        /// </summary>
        /// <param name="reader"></param>
        public void PushReader(TextReader reader, string path)
        {
            if (paths.Contains(path))
            {
                throw new ConfigException("The path {0} is already loaded. There are cycles in the configuration.");
            }

            readers.Push(reader);
            paths.Push(path);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach(var reader in readers)
            {
                reader.Dispose();
            }

            readers.Clear();
        }

        #endregion
    }
}
