using System;
using System.IO;
using System.Text.RegularExpressions;
using Ini.EventLogs;
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
        /// <param name="reader"></param>
		/// <param name="configEventLog"></param>
		/// <param name="specEventLog"></param>
		/// <param name="mode"></param>
		/// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
		/// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
		/// <exception cref="MalformedConfigException">If the configuration's format is malformed.</exception>
        /// <returns></returns>
		public Config Parse(TextReader reader, IConfigReaderEventLog configEventLog, ISpecValidatorEventLog specEventLog, ConfigValidationMode mode)
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
            return config;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="config"></param>
		/// <param name="configEventLog"></param>
		/// <param name="specEventLog"></param>
		/// <param name="mode"></param>
		/// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
		/// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
		/// <exception cref="MalformedConfigException">If the configuration's format is malformed.</exception>
        /// <returns></returns>
		public bool TryParse(TextReader reader, out Config config, IConfigReaderEventLog configEventLog, ISpecValidatorEventLog specEventLog, ConfigValidationMode mode)
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

        #region Private Methods

		private void ParseOption(string option, int lineIndex, Section section, IConfigReaderEventLog eventLog)
        {
            string[] splitted = option.Split(new char[] { INNER_OPTION_SEPARATOR }, 2); // splits by first occurrence and limits to two substrings
            if(splitted.Length == 2)
            {
                string optionIdentifier = TrimWhitespaces(splitted[0]);
                string optionValue = TrimWhitespaces(splitted[1]);

                if(IsIdentifierWellFormed(optionIdentifier, IdentifierType.OPTION))
                {
                    ParseElement(lineIndex, optionIdentifier, optionValue, section, eventLog);
                }
                else
                {
                    // TODO: maybe more elaborate?
                    eventLog.ConfigMalformed(lineIndex, string.Format("Error at line {0}: identifier is not well formed.", lineIndex));
                }
            }
            else
            {
                eventLog.ConfigMalformed(lineIndex, string.Format("Error at line {0}: can not parse option because of missing '{1}'.", lineIndex, INNER_OPTION_SEPARATOR));
            }
        }

		private void ParseElement(int lineIndex, string identifier, string value, Section section, IConfigReaderEventLog eventLog)
        {
            // hodnota je reprezentována jedním nebo více elementy stejného typu oddělených čárkou (,) nebo dvojtečkou (:), v rámci jedné hodnoty ale vždy buď pouze (,) nebo pouze (:)
            // TODO: semicolon shall be preferred over comma but don't forget about escaping with slashes...
            // TODO: type checks

            // Option option = null; // TODO
            // section.Options.Add(identifier, option);
        }

        private void TypeCheck()
        {
            // TODO:
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
            // mezery na začátku identifikátoru nebo volby se ignorují, pokud jim nepředchází znak '\'
            return null;
        }

        /// <summary>
        /// Removes all trailing whitespaces from a config token, as per the specification - stops if a whitespace prefixed
        /// with a slash is encountered.
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim trailing whitespaces from.</param>
        private static string TrimTrailingWhitespaces(string token)
        {
            // mezery na konci identifikátoru nebo volby se ignorují, pokud jim nepředchází znak '\'
            return null;
        }

        /// <summary>
        /// Removes trailing commentary from a config token, as per the specification. After that, trims trailing whitespaces.
        /// </summary>
        /// <returns>The given token, with trailing commentary removed.</returns>
        /// <param name="token">The token to remove trailing commentary from.</param>
        private static string RemoveTrailingCommentary(string token)
        {
            return null;
        }

        #endregion
    }
}
