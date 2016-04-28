using System;
using System.IO;
using System.Text.RegularExpressions;
using Ini.EventLoggers;
using Ini.Configuration;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using Ini.Exceptions;

namespace Ini
{
    /// <summary>
    /// The class that parses the configuration from text.
    /// </summary>
    public class ConfigParser
    {
        #region Constants

        const string IDENTIFIER_START_CHAR_REGEX = "[a-zA-Z.:$]";
        const string IDENTIFIER_SUFFIX_CHAR_REGEX = "[a-zA-Z0-9_~\\-.:$ ]";
        const string IDENTIFIER_REGEX = IDENTIFIER_START_CHAR_REGEX + IDENTIFIER_SUFFIX_CHAR_REGEX + "*";

        const string OPTION_IDENTIFIER_REGEX = "^" + IDENTIFIER_REGEX + "$";
        const string SECTION_IDENTIFIER_REGEX = "^\\[" + IDENTIFIER_REGEX + "\\]$";

        const char INNER_OPTION_SEPARATOR = '=';
        const char COMMENTARY_STARTER = ';';
        const string ESCAPE_SEQUENCE = "\\";

        /// <summary>
        /// Use the first capture group to match the filtered string token. Meaning of the regex:
        /// leading whitespaces are ignored, up until the first non-whitespace character.
        /// </summary>
        const string FILTER_LEADING_WHITESPACES_REGEX = "^\\s*(.*?)$";

        /// <summary>
        /// Use the first capture group to match the filtered string token. Meaning of the regex:
        /// trailing whitespaces are ignored, up until the last one prefixed with <see cref="ESCAPE_SEQUENCE"/>.
        /// </summary>
        const string FILTER_TRAILING_WHITESPACES_REGEX = "^(.*?(?:" + ESCAPE_SEQUENCE + "\\s)?)\\s*$";

        #endregion

        #region Properties

        /// <summary>
        /// The configuration being parsed. It's purpose is to preserve what's parsed
        /// when an exception occurs.
        /// </summary>
        protected Config config;

        #endregion

        #region Private types

        enum IdentifierType
        {
            OPTION,
            SECTION
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigParser"/> class.
        /// </summary>
        /// <param name="spec">The specification to validate against when parsing the configuration.</param>
        public ConfigParser(ConfigSpec spec)
        {
            this.config = new Config(spec);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Parses the configuration from the text input.
        /// </summary>
        /// <param name="reader">The reader with configuration file</param>
        /// <param name="configEventLog">The config reader event log.</param>
        /// <param name="specEventLog">The spec validator event log.</param>
        /// <param name="mode">The validation mode.</param>
        /// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
        /// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
        /// <exception cref="MalformedConfigException">If the configuration's format is malformed.</exception>
        /// <returns></returns>
        public Config Parse(TextReader reader, IConfigReaderEventLogger configEventLog, ISpecValidatorEventLogger specEventLog, ConfigValidationMode mode)
        {
            // check preconditions
            if(mode == ConfigValidationMode.Strict) // we need a valid specification
            {
                if(config.Spec == null) // and we have none
                {
                    configEventLog.SpecNotFound();
                    throw new UndefinedSpecException();
                }
                if(!config.Spec.IsValid(specEventLog)) // we have one but it's not valid
                {
                    configEventLog.SpecNotValid();

                    // raise an exception or face undefined behaviour
                    throw new InvalidSpecException();
                }
            }

            // either way, now we have a valid specification or don't need one
            string line;
            int lineIndex = 0;
            while((line = reader.ReadLine()) != null)
            {
                // preparation
                lineIndex++;
                line = TrimWhitespaces(RemoveTrailingCommentary(line));

                // handle sections
                if(IsIdentifierWellFormed(line, IdentifierType.SECTION))
                {
                    // string identifier = TrimWhitespaces(line.Substring(1, line.Length - 2));

                }
            }

            // PSEUDOCODE:
            /*
             * 1. Read a line from 'reader', until there is none left.
             * 2. 	- Increment 'lineIndex'.
             * 3.	- Trim commentary and whitespaces from the line.
             * 4. 	- If the result doesn't match a section declaration, try parsing it as an option declaration.
             * 5. 		- If successful, invoke 'ParseOption(option, section, backlog)'.
             * 6. 		- If not successful, report with 'backlog'.
             * 7.	- If the result matches a section declaration:
             * 8.		- Parse identifier from it and trim whitespaces.
             * 9. 		- If a section with the same identifier is already registered, report with 'backlog'.
             * 10.		- Otherwise:
             * 11.			- section = new Section(identifier);
             * 12.			- result.Add(section);
             * 
             * Meanwhile, catch exceptions and report with 'backlog'.
             */

			/*
			 * After that, we need to mind links and divide the parsing process into phases:
			 * 1) First parse every option and element as string.
			 * 2) Look at the string values, find links and register them into a resolver instance.
			 * 3) Once we have all sections and options parsed like that, resolve links on the resolver.
			 * 4) And finally, we can interpret the option values in their correct contexts.
			 */
            return config;
        }

        /// <summary>
        /// Parses the configuration from the text input.
        /// </summary>
        /// <param name="reader">The reader with configuration file</param>
        /// <param name="config">The parsed configuration file.</param>
        /// <param name="configEventLog">The config reader event log.</param>
        /// <param name="specEventLog">The spec validator event log.</param>
        /// <param name="mode">The validation mode.</param>
        /// <returns>True if the configuration is parsed successfully.</returns>
        public bool TryParse(TextReader reader, out Config config, IConfigReaderEventLogger configEventLog, ISpecValidatorEventLogger specEventLog, ConfigValidationMode mode)
        {
            try
            {
                config = Parse(reader, configEventLog, specEventLog, mode);
                return true;
            }
            catch (Exception)
            {
                config = this.config; // return what was parsed before an exception was thrown
                return false;
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Checks that the given token is a valid identifier. Automatically handles leading/trailing whitespaces
        /// and trailing commentary. All in conformance with the specification.
        /// 
        /// Note: for the sake of performance, it is better to pass an already preprocessed input. Typically, you'll
        /// probably want to save it right afterwards.
        /// </summary>
        /// <returns><c>true</c> If the given token is a well formed identifier, as per the current config policy,
        /// otherwise <c>false</c>.</returns>
        /// <param name="token">The token to check.</param>
        /// <param name="identifierType"></param>
        private static bool IsIdentifierWellFormed(string token, IdentifierType identifierType)
        {
            // remove trailing commentary
            // trim whitespaces
            // and finally:
            switch (identifierType)
            {
                case IdentifierType.OPTION:
                    return Regex.IsMatch(token, OPTION_IDENTIFIER_REGEX);
                case IdentifierType.SECTION:
                    return Regex.IsMatch(token, SECTION_IDENTIFIER_REGEX);
                default:
                    throw new ArgumentException(); // TODO: is there a better exception?
            }
        }

        /// <summary>
        /// Removes all leading/trailing whitespaces from a config token. See:
        /// TrimLeadingWhitespaces
        /// TrimTrailingWhitespaces
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim whitespaces from.</param>
        private static string TrimWhitespaces(string token)
        {
            return TrimLeadingWhitespaces(TrimTrailingWhitespaces(token));
        }

        /// <summary>
        /// Removes all leading whitespaces from a config token, as per the specification - stops if a whitespace prefixed
        /// with a slash is encountered.
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim leading whitespaces from.</param>
        private static string TrimLeadingWhitespaces(string token)
        {
            Match match = Regex.Match(token, FILTER_LEADING_WHITESPACES_REGEX);
            return match.Success ? match.Groups[1].Value : token;
        }

        /// <summary>
        /// Removes all trailing whitespaces from a config token, as per the specification - stops if a whitespace prefixed
        /// with a slash is encountered.
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim trailing whitespaces from.</param>
        private static string TrimTrailingWhitespaces(string token)
        {
            Match match = Regex.Match(token, FILTER_TRAILING_WHITESPACES_REGEX);
            return match.Success ? match.Groups[1].Value : token;
        }

        /// <summary>
        /// Removes trailing commentary from a config token, as per the specification.
        /// </summary>
        /// <returns>The given token, with trailing commentary removed.</returns>
        /// <param name="token">The token to remove trailing commentary from.</param>
        private static string RemoveTrailingCommentary(string token)
        {
            int firstSemicolonIndex = token.IndexOf(COMMENTARY_STARTER);
            return firstSemicolonIndex == -1 ? token : token.Substring(0, firstSemicolonIndex);
        }

        #endregion
    }
}
