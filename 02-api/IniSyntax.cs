using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Ini.Configuration.Base;
using Ini.Configuration;
using Ini.Util.LinkResolving;
using System.Text;

namespace Ini
{
    /// <summary>
    /// Ini syntax.
    /// </summary>
    public class IniSyntax
    {
        #region Constants

        const string ESCAPE_SEQUENCE = "\\";

        const string PRIMARY_ELEMENT_SEPARATOR = ",";
        const string SECONDARY_ELEMENT_SEPARATOR = ":";
        const string COMMENTARY_SEPARATOR = ";";
        const char LINK_IDENTIFIER_SEPARATOR = '#';

        const string IDENTIFIER_START_CHAR_REGEX = "[a-zA-Z.$:]";
        const string IDENTIFIER_SUFFIX_CHAR_REGEX = "[a-zA-Z0-9_~\\-.:$ " + ESCAPE_SEQUENCE + ESCAPE_SEQUENCE + "]";
        const string IDENTIFIER_REGEX = IDENTIFIER_START_CHAR_REGEX + IDENTIFIER_SUFFIX_CHAR_REGEX + "*";

        const string LINE_REGEX_SECTION = "^\\[" + IDENTIFIER_REGEX + "\\]$";
        const string LINE_REGEX_OPTION = "^(" + IDENTIFIER_REGEX + ")" + "(=(?:" + SECONDARY_ELEMENT_SEPARATOR + "=)?)" + "(.*)$";

        const string LINK_REGEX = "^\\${([^}]+)}$";

        #endregion

        #region Special types

        /// <summary>
        /// All known lines of content.
        /// </summary>
        public enum LineContent
        {
            /// <summary>
            /// Section header line.
            /// </summary>
            SECTION_HEADER,

            /// <summary>
            /// Option line.
            /// </summary>
            OPTION
        }

        #endregion

        #region Query Helpers

        /// <summary>
        /// Determines whether the specified line of input matches the specified content.
        /// </summary>
        /// <returns><c>true</c> If the specified line matches the specified content, otherwise <c>false</c>.</returns>
        /// <param name="line">The input line.</param>
        /// <param name="lineContent">The target content.</param>
        public static bool LineMatches(string line, LineContent lineContent)
        {
            switch (lineContent)
            {
                case LineContent.SECTION_HEADER:
                    return Regex.IsMatch(line, LINE_REGEX_SECTION);    
                case LineContent.OPTION:
                    return Regex.IsMatch(line, LINE_REGEX_OPTION);
                default:
                    throw new ArgumentException("Unknown enum value: " + lineContent.ToString());
            }
        }

        /// <summary>
        /// Determines whether the specified element is a link.
        /// </summary>
        /// <returns><c>true</c> if is the element is a link; otherwise, <c>false</c>.</returns>
        /// <param name="element">The element.</param>
        public static bool IsElementALink(string element)
        {
            // content must not be empty because identifiers must not be empty as well
            return Regex.Match(TrimWhitespaces(element), UnescapedTokenRegex(LINK_REGEX)).Success;
        }

        #endregion

        #region Deserialization Helpers

        /// <summary>
        /// Extracts section identifier from the input line (section header) and returns it.
        /// </summary>
        /// <returns>The extracted identifier.</returns>
        /// <param name="line">The input line.</param>
        /// <exception cref="ArgumentException">If the input line doesn't match a section header.</exception>
        public static string ExtractSectionId(string line)
        {
            if(LineMatches(line, LineContent.SECTION_HEADER))
            {
                return TrimWhitespaces(line.Substring(1, line.Length - 2));
            }
            else
            {
                throw new ArgumentException("The specified line doesn't match a section header.");
            }
        }

        /// <summary>
        /// Extracts individual option components from the specified input line.
        /// </summary>
        /// <param name="line">The input line.</param>
        /// <param name="identifier">The extracted identifier.</param>
        /// <param name="separator">The extracted separator.</param>
        /// <param name="value">The extracted value.</param>
        /// <exception cref="ArgumentException">If the input line doesn't match option syntax or it defines an empty identifier.</exception>
        public static void ExtractOptionComponents(string line, out string identifier, out string separator, out string value)
        {
            line = TrimWhitespaces(line);
            Match match = Regex.Match(line, LINE_REGEX_OPTION);
            if(match.Success)
            {
                identifier = TrimTrailingWhitespaces(match.Groups[1].Value); // the specification tells us to do so
                separator = match.Groups[2].Value.Contains(SECONDARY_ELEMENT_SEPARATOR) ? SECONDARY_ELEMENT_SEPARATOR : PRIMARY_ELEMENT_SEPARATOR;
                value = match.Groups[3].Value; // must not explicitly trim leading whitespaces of a value
                if(string.IsNullOrEmpty(identifier))
                {
                    throw new ArgumentException("Library specification states that identifiers must not be empty.");
                }
            }
            else
            {
                throw new ArgumentException("The specified line doesn't match option syntax.");
            }
        }

        /// <summary>
        /// Extracts individual elements from the specified complete option value.
        /// </summary>
        /// <returns>The elementary elements.</returns>
        /// <param name="value">The complete value of an option.</param>
        /// <param name="separator">The separator for elements.</param>
        public static IEnumerable<string> ExtractAndUnescapeElements(string value, string separator)
        {
            // prepare the result enumerable
            List<string> result = new List<string>();

            // iterate through all occurrences of the element separator (if unescaped)
            int elementStartIndex = 0;
            foreach(Match elementMatch in Regex.Matches(value, UnescapedTokenRegex(separator)))
            {
                string element = value.Substring(elementStartIndex, elementMatch.Index);
                result.Add(UnescapeElement(element));
                elementStartIndex = elementMatch.Index;
            }

            // and the remainder
            result.Add(value.Substring(elementStartIndex, value.Length - elementStartIndex));

            // and return
            return result;
        }

        /// <summary>
        /// Extracts individual components from the specified link.
        /// </summary>
        /// <returns>The components.</returns>
        /// <param name="link">The link to parse.</param>
        /// <exception cref="ArgumentException">If the input token doesn't match a link syntax.</exception>
        public static string[] ExtractLinkComponents(string link)
        {
            // content must not be empty because identifiers must not be empty as well
            Match match = Regex.Match(TrimWhitespaces(link), UnescapedTokenRegex(LINK_REGEX));
            if(match.Success)
            {
                // use the first capture group to fetch the link's content
                return match.Groups[1].Value.Split(LINK_IDENTIFIER_SEPARATOR);
            }
            else
            {
                throw new ArgumentException("The specified string doesn't match a link syntax.");
            }
        }

        /// <summary>
        /// Extracts trailing commentary from the specified line into the specified output
        /// parameter. Returns the line's prefix, up to the start of the commentary.
        /// </summary>
        /// <returns>The line's prefix, up to the start of the commentary.</returns>
        /// <param name="line">The input line.</param>
        /// <param name="commentary">The extracted trailing commentary, if any.</param>
        public static string ExtractAndRemoveCommentary(string line, out string commentary)
        {
            // match the first semicolon that is not preceded by an escape sequence
            Match match = Regex.Match(line, UnescapedTokenRegex(COMMENTARY_SEPARATOR));
            if(match.Success)
            {
                commentary = line.Substring(match.Index + 1, line.Length - match.Index - 1);
                return line.Substring(0, match.Index);
            }
            else
            {
                commentary = null;
                return line;
            }
        }

        #endregion

        #region Serialization Helpers

        /// <summary>
        /// Serializes the specified commentary (prefixes it with the commentary separator).
        /// </summary>
        /// <returns>The serialized comment.</returns>
        /// <param name="commentary">The commentary.</param>
        public static string SerializeCommentary(string commentary)
        {
            if (string.IsNullOrWhiteSpace(commentary))
            {
                return "";
            }
            else
            {
                return string.Format("{0} {1}", COMMENTARY_SEPARATOR, commentary);
            }
        }

        /// <summary>
        /// Serializes the specified section header (encloses it in brackets).
        /// </summary>
        /// <returns>The serialized section identifier.</returns>
        /// <param name="identifier">The section identifier.</param>
        public static string SerializeSectionHeader(string identifier)
        {
            return '[' + identifier + ']';
        }

        /// <summary>
        /// Serializes the specified option identifier and its elements.
        /// </summary>
        /// <returns>The serialized option.</returns>
        /// <param name="identifier">The option's identifier.</param>
        /// <param name="elements">The option's value elements.</param>
        /// <param name="configuration">The parent configuration.</param>
        public static string SerializeOption(string identifier, IEnumerable<IElement> elements, Config configuration)
        {
            return string.Format("{0} = {1}", identifier, SerializeElements(elements, configuration));
        }

        /// <summary>
        /// Serializes the specified elements.
        /// </summary>
        /// <returns>The serialized elements.</returns>
        /// <param name="elements">The elements to serialize.</param>
        /// <param name="configuration">The parent configuration.</param>
        public static string SerializeElements(IEnumerable<IElement> elements, Config configuration)
        {
            return string.Join(PRIMARY_ELEMENT_SEPARATOR, elements.Select(item => EscapeElement(item.ToOutputString(configuration))));
        }

        /// <summary>
        /// Constructs the string representation of a link from the specified target.
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="target">The target.</param>
        public static string SerializeLink(LinkTarget target)
        {
            return string.Format("${{{0}{1}{2}}}", target.Section, LINK_IDENTIFIER_SEPARATOR, target.Option);
        }

        #endregion

        #region General Helpers

        /// <summary>
        /// Removes all leading/trailing whitespaces from a config token. See:
        /// TrimLeadingWhitespaces
        /// TrimTrailingWhitespaces
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim whitespaces from.</param>
        public static string TrimWhitespaces(string token)
        {
            return TrimLeadingWhitespaces(TrimTrailingWhitespaces(token));
        }

        /// <summary>
        /// Removes all leading whitespaces from a config token, as per the specification - stops if a whitespace prefixed
        /// with a slash is encountered.
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim leading whitespaces from.</param>
        public static string TrimLeadingWhitespaces(string token)
        {
            // use the first capture group to match the filtered string token
            // meaning: leading whitespaces are ignored, up until the first non-whitespace character

            Match match = Regex.Match(token, "^\\s*(.*?)$");
            return match.Success ? match.Groups[1].Value : token;
        }

        /// <summary>
        /// Removes all trailing whitespaces from a config token, as per the specification - stops if a whitespace prefixed
        /// with a slash is encountered.
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim trailing whitespaces from.</param>
        public static string TrimTrailingWhitespaces(string token)
        {
            // use the first capture group to match the filtered string token
            // meaning: trailing whitespaces are ignored, up until the last one that is escaped

            Match match = Regex.Match(token, "^(.*?(?:" + ESCAPE_SEQUENCE + ESCAPE_SEQUENCE + "\\s)?)\\s*$");
            return match.Success ? match.Groups[1].Value : token;
        }

        /// <summary>
        /// Builds a regular expression that matches the specified token as long as it
        /// is not preceded by an escape sequence (<see cref="ESCAPE_SEQUENCE"/>).
        /// </summary>
        /// <returns>The regular expression.</returns>
        /// <param name="token">The token.</param>
        protected static string UnescapedTokenRegex(string token)
        {
            return "(?<!" + ESCAPE_SEQUENCE + ESCAPE_SEQUENCE + ")" + token;
        }

        /// <summary>
        /// Escapes meta-characters in the specified element.
        /// </summary>
        /// <returns>The element to escape.</returns>
        /// <param name="element">The element, with escaped meta-characters.</param>
        protected static string EscapeElement(string element)
        {
            StringBuilder builder = new StringBuilder();
            foreach(char c in element)
            {
                string cs = c.ToString();
                if((cs == PRIMARY_ELEMENT_SEPARATOR) || (cs == SECONDARY_ELEMENT_SEPARATOR) || (cs == COMMENTARY_SEPARATOR))
                {
                    builder.Append(ESCAPE_SEQUENCE);
                }
                builder.Append(c);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Unescapes meta-characters in the specified element.
        /// </summary>
        /// <returns>The element to unescape.</returns>
        /// <param name="element">The element, with unescaped meta-characters.</param>
        protected static string UnescapeElement(string element)
        {
            return Regex.Replace(
                element,
                ESCAPE_SEQUENCE + ESCAPE_SEQUENCE + "([" + PRIMARY_ELEMENT_SEPARATOR + SECONDARY_ELEMENT_SEPARATOR + COMMENTARY_SEPARATOR + "])",
                new MatchEvaluator(match => match.Groups[1].Value));
        }

        #endregion
    }
}
