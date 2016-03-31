﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_api_alternative.Definitions;
using _02_api_alternative.Interfaces;

namespace _02_api_alternative
{
    /// <summary>
    /// The class used to read configuration from various sources.
    /// </summary>
    public class ConfigurationReader
    {
        /// <summary>
        /// A user-specified or default backlog for handling errors and parsing messages.
        /// </summary>
        private IParsingBacklog backlog;

        public ConfigurationReader()
        {
            this.backlog = new ConsoleParsingBacklog();
        }

        public ConfigurationReader(IParsingBacklog backlog)
        {
            this.backlog = backlog;
        }

        /// <summary>
        /// Creates an instance of <see cref="Config"/> from the given path, using the system-default encoding.
        /// </summary>
        /// <returns>The config read and parsed from the given path.</returns>
        /// <param name="path">The given path.</param>
        public Configuration LoadFromFile(string path, ValidationMode mode, Schema schema = null)
        {
            return LoadFromFile(path, mode, Encoding.Default, schema);
        }

        //Doplnit ostatní varianty!!!
        public bool TryLoadFromFile(string path, ValidationMode mode, out Configuration configuration, Schema schema = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an instance of <see cref="Config"/> from the given path and encoding.
        /// </summary>
        /// <returns>The config read and parsed from the given path and encoding.</returns>
        /// <param name="path">The given path.</param>
        /// <param name="encoding">The given encoding.</param>
        public Configuration LoadFromFile(string path, ValidationMode mode, Encoding encoding = null, Schema schema = null)
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return LoadFromText(new StreamReader(fileStream, encoding), mode, schema);
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="Config"/> from the given reader. Can be used to load a config
        /// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
        /// </summary>
        /// <returns>The config read and parsed from the given reader.</returns>
        /// <param name="reader">The given reader.</param>
        public Configuration LoadFromText(TextReader reader, ValidationMode mode, Schema schema = null)
        {
            ConfigurationParser parser = new ConfigurationParser();
            return parser.Parse(reader, backlog, mode);
        }

    }
}
