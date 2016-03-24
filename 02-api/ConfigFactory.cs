using System;
using System.IO;
using System.Text;

namespace IniConfig
{
	/// <summary>
	/// A wrapper class to create instances of <see cref="Config"/>, with custom error output.
	/// </summary>
	public class ConfigFactory
	{
		/// <summary>
		/// A user-specified or default backlog for handling errors and parsing messages.
		/// </summary>
		private IConfigBacklog backlog;

		public ConfigFactory()
		{
			this.backlog = new ConsoleBacklog();
		}

		public ConfigFactory(IConfigBacklog backlog)
		{
			this.backlog = backlog;
		}

		/// <summary>
		/// Creates an instance of <see cref="Config"/> from the given path, using the system-default encoding.
		/// </summary>
		/// <returns>The config read and parsed from the given path.</returns>
		/// <param name="path">The given path.</param>
		public Config LoadFromFile(string path)
		{
			return LoadFromFile(path, Encoding.Default);
		}

		/// <summary>
		/// Creates an instance of <see cref="Config"/> from the given path and encoding.
		/// </summary>
		/// <returns>The config read and parsed from the given path and encoding.</returns>
		/// <param name="path">The given path.</param>
		/// <param name="encoding">The given encoding.</param>
		public Config LoadFromFile(string path, Encoding encoding)
		{
			try
			{
				using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					return LoadFromReader(new StreamReader(fileStream, encoding));
				}
			}
			catch (Exception e)
			{
				backlog.ParsingException(e);
				return null;
			}
		}

		/// <summary>
		/// Creates an instance of <see cref="Config"/> from the given reader. Can be used to load a config
		/// from a file or memory. Use the other ready-to-use factory methods for loading config from files, however.
		/// </summary>
		/// <returns>The config read and parsed from the given reader.</returns>
		/// <param name="reader">The given reader.</param>
		public Config LoadFromReader(StreamReader reader)
		{
			try
			{
				ConfigParser parser = new ConfigParser();
				return parser.Parse(reader, backlog);
			}
			catch (Exception e)
			{
				backlog.ParsingException(e);
				return null;
			}
		}
	}
}
