using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ini.EventLoggers;
using Ini.Configuration;
using Ini.Specification;
using Ini.Util;
using Ini.Validation;
using Ini.Exceptions;
using Ini.Configuration.Values;
using Ini.Util.LinkResolving;

namespace Ini
{
    /// <summary>
    /// A one-time-use class to parse a configuration from text.
    /// </summary>
    public class ConfigParser
    {
        #region Constants

        const string INNER_OPTION_SEPARATOR = "=";
        const string COMMENTARY_STARTER = ";";
        const string ESCAPE_SEQUENCE = "\\";

        const string IDENTIFIER_START_CHAR_REGEX = "[a-zA-Z.:$]";
        const string IDENTIFIER_SUFFIX_CHAR_REGEX = "[a-zA-Z0-9_~\\-.:$\\ ]";
        const string IDENTIFIER_REGEX = IDENTIFIER_START_CHAR_REGEX + IDENTIFIER_SUFFIX_CHAR_REGEX + "*";

        const string LINE_REGEX_OPTION = "^(" + IDENTIFIER_REGEX + ")" + INNER_OPTION_SEPARATOR + "(.*)$";
        const string LINE_REGEX_SECTION = "^\\[" + IDENTIFIER_REGEX + "\\]$";

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

        #region Special types

        /// <summary>
        /// Basic information about a line of input.
        /// </summary>
        protected class LineInfo
        {
            /// <summary>
            /// The line itself, with the trailing commentary removed and then trimmed.
            /// </summary>
            /// <value>The line.</value>
            public string Line { get; private set; }

            /// <summary>
            /// The trailing commentary extracted from <see cref="Line"/>.
            /// </summary>
            /// <value>The trailing commentary.</value>
            public string TrailingCommentary { get; private set; }

            /// <summary>
            /// Determines whether <see cref="Line"/> represents a section header.
            /// </summary>
            /// <value><c>true</c> if line is a section header; otherwise, <c>false</c>.</value>
            public bool IsSection { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="LineInfo"/> class.
            /// </summary>
            /// <param name="line">The currently processed line of input.</param>
            public LineInfo(string line)
            {
                string trailingCommentary;
                this.Line = TrimWhitespaces(ExtractTrailingCommentary(line, out trailingCommentary));
                this.TrailingCommentary = trailingCommentary;
                this.IsSection = Matches(Line, LineContent.SECTION_HEADER);
            }
        }

        /// <summary>
        /// Basic information about a line of input.
        /// </summary>
        protected class ParserContext : IEnumerable<LineInfo>
        {
            /// <summary>
            /// Current parsing mode.
            /// </summary>
            public ParserMode Mode { get; set; }

            /// <summary>
            /// Currently parsed section.
            /// </summary>
            public Section CurrentSection { get; set; }

            /// <summary>
            /// Number of the currently parsed line. Starts from 1.
            /// </summary>
            /// <value>The line number.</value>
            public int LineNumber { get { return CurrentLinesIndex + 1; } }

            /// <summary>
            /// Lines to parse.
            /// </summary>
            protected List<LineInfo> Lines;

            /// <summary>
            /// Index of the currently processed line.
            /// </summary>
            protected int CurrentLinesIndex;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParserContext"/> class.
            /// </summary>
            public ParserContext(List<LineInfo> lines)
            {
                this.Mode = ParserMode.DO_NOT_SKIP;
                this.CurrentSection = null;
                this.Lines = lines;
                this.CurrentLinesIndex = 0;
            }

            #region IEnumerable implementation

            /// <summary>
            /// Gets the enumerator.
            /// </summary>
            /// <returns>The enumerator.</returns>
            public IEnumerator<LineInfo> GetEnumerator()
            {
                while(CurrentLinesIndex < Lines.Count)
                {
                    switch(Mode)
                    {
                        case ParserMode.SKIP_UNTIL_NEXT_SECTION:
                            if(Lines[CurrentLinesIndex].IsSection)
                            {
                                Mode = ParserMode.DO_NOT_SKIP;
                                yield return Lines[CurrentLinesIndex];
                            }
                            break;

                        case ParserMode.DO_NOT_SKIP:
                            yield return Lines[CurrentLinesIndex];
                            break;

                        default:
                            throw new ArgumentException("Unknown enum value: " + Mode.ToString());
                    }
                    CurrentLinesIndex++;
                }
                yield break;
            }

            #endregion

            #region IEnumerable implementation

            /// <summary>
            /// Gets the enumerator.
            /// </summary>
            /// <returns>The enumerator.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        /// <summary>
        /// The parsing mode when processing lines.
        /// </summary>
        protected enum ParserMode
        {
            /// <summary>
            /// Skip all further lines, until you come up to a next section.
            /// </summary>
            SKIP_UNTIL_NEXT_SECTION,

            /// <summary>
            /// Don't skip any line and try to parse it.
            /// </summary>
            DO_NOT_SKIP
        }

        /// <summary>
        /// All known lines of content.
        /// </summary>
        protected enum LineContent
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

        #region Properties

        /// <summary>
        /// The configuration being parsed.
        /// </summary>
        protected ConfigSpec specification;

        /// <summary>
        /// The configuration being parsed.
        /// </summary>
        protected Config config;

        /// <summary>
        /// Link resolver to use when parsing the configuration.
        /// </summary>
        protected LinkResolver linkResolver;

        /// <summary>
        /// Basic information about lines of input.
        /// </summary>
        protected List<LineInfo> lines;

        /// <summary>
        /// The validation mode to apply when parsing the configuration.
        /// </summary>
        protected ConfigValidationMode validationMode;

        /// <summary>
        /// Event logger to call when validating the specification.
        /// </summary>
        protected ISpecValidatorEventLogger specEventLog;

        /// <summary>
        /// Event logger to call when reading the configuration.
        /// </summary>
        protected IConfigReaderEventLogger configEventLog;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.ConfigParser"/> class.
        /// </summary>
        /// <param name="specification">The specification to validate against when parsing the configuration.</param>
        public ConfigParser(ConfigSpec specification)
        {
            this.specification = specification;
            this.config = new Config(specification);
            this.linkResolver = new LinkResolver();
            this.lines = new List<LineInfo>();
            this.validationMode = ConfigValidationMode.Strict;
            this.specEventLog = null;
            this.configEventLog = null;
        }

        #endregion

        #region Top parsing methods

        /// <summary>
        /// Parses the configuration from the text input.
        /// </summary>
        /// <param name="input">The reader with configuration file</param>
        /// <param name="configEventLog">The config reader event log.</param>
        /// <param name="specEventLog">The spec validator event log.</param>
        /// <param name="validationMode">The validation mode.</param>
        /// <exception cref="Ini.Exceptions.UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
        /// <exception cref="Ini.Exceptions.InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
        /// <exception cref="MalformedConfigException">If the configuration's format is malformed.</exception>
        /// <returns></returns>
        public Config Parse(TextReader input, IConfigReaderEventLogger configEventLog, ISpecValidatorEventLogger specEventLog, ConfigValidationMode validationMode)
        {
            // prepare fields (forward arguments)
            this.configEventLog = configEventLog;
            this.specEventLog = specEventLog;
            this.validationMode = validationMode;

            // and continue
            PreconditionsTrue();
            PreprocessLines(input);
            ParseLines();
            linkResolver.ResolveLinks(config, configEventLog);

            // TODO: interpret the option values in their correct contexts

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

        #region Lower-level parsing methods

        /// <summary>
        /// Check preconditions for parsing.
        /// </summary>
        protected void PreconditionsTrue()
        {
            if(validationMode == ConfigValidationMode.Strict) // we need a valid specification
            {
                if(config.Spec == null) // and we have none
                {
                    configEventLog.NoSpecification();
                    throw new UndefinedSpecException();
                }
                if(!config.Spec.IsValid(specEventLog)) // we have one but it's not valid
                {
                    configEventLog.SpecificationNotValid();

                    // raise an exception or face undefined behaviour
                    throw new InvalidSpecException();
                }
            }
        }

        /// <summary>
        /// Reads the specified input into <see cref="lines"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        protected void PreprocessLines(TextReader input)
        {
            string line;
            while((line = input.ReadLine()) != null)
            {
                lines.Add(new LineInfo(line));
            }
        }

        /// <summary>
        /// Parse the whole input into <see cref="config"/> while also filling up
        /// <see cref="linkResolver"/>.
        /// </summary>
        protected void ParseLines()
        {
            // prepare
            ParserContext context = new ParserContext(lines);
            List<string> filler = new List<string>();

            // let the parser choose the right lines to parse
            foreach(LineInfo lineInfo in context) 
            {
                if(string.IsNullOrEmpty(lineInfo.Line))
                {
                    // comments or empty lines must be saved for later
                    filler.Add(lineInfo.TrailingCommentary == null ? "" : lineInfo.TrailingCommentary);
                }
                else
                {
                    // first register the saved comments or empty lines, if needed
                    if(filler.Count > 0)
                    {
                        config.Add(new Commentary(filler));
                        filler.Clear();
                    }

                    // and then continue parsing
                    if(lineInfo.IsSection)
                    {
                        ParseSectionHeader(context, lineInfo);
                    }
                    else if(Matches(lineInfo.Line, LineContent.OPTION))
                    {
                        ParseOption(context, lineInfo);
                    }
                    else
                    {
                        // report unknown syntax
                        configEventLog.UnknownLineSyntax(context.LineNumber, lineInfo.Line);

                        // and place an empty line instead
                        config.Add(new Commentary(new string[] { "" }));
                    }
                }
            }
        }

        /// <summary>
        /// Parse the current line as a section header.
        /// </summary>
        /// <param name="context">Parsing context.</param>
        /// <param name="lineInfo">Information about the current line.</param>
        protected void ParseSectionHeader(ParserContext context, LineInfo lineInfo)
        {
            string identifier = TrimWhitespaces(lineInfo.Line.Substring(1, lineInfo.Line.Length - 2));
            if(config.Contains(identifier))
            {
                configEventLog.DuplicateSection(context.LineNumber, identifier);
                context.Mode = ParserMode.SKIP_UNTIL_NEXT_SECTION;
            }
            else
            {
                Section newSection = new Section(identifier, lineInfo.TrailingCommentary);
                config.Add(newSection);
                context.CurrentSection = newSection;
            }
        }

        /// <summary>
        /// Parse the current line as an option.
        /// </summary>
        /// <param name="context">Parsing context.</param>
        /// <param name="lineInfo">Information about the current line.</param>
        protected void ParseOption(ParserContext context, LineInfo lineInfo)
        {
            Match match = Regex.Match(lineInfo.Line, LINE_REGEX_OPTION);
            if(match.Success)
            {
                // parse the option
                string identifier = match.Groups[1].Value;
                string value = match.Groups[2].Value;

                // check duplicates
                if(context.CurrentSection.Contains(identifier))
                {
                    configEventLog.DuplicateOption(context.LineNumber, identifier);
                }
                else
                {
                    // TODO:
                    // Type optionType = validationMode == ConfigValidationMode.Strict ? specification.
                }
            }
            else
            {
                throw new InvalidOperationException("This method can not be called if the line doesn't match the regular expression for options.");
            }
        }

        #endregion

        #region Static helpers

        /// <summary>
        /// Determines whether the specified token matches the specified content.
        /// </summary>
        /// <returns><c>true</c> If the specified token matches the specified content, otherwise <c>false</c>.</returns>
        /// <param name="token">The token to match.</param>
        /// <param name="lineContent">What to match the token with.</param>
        protected static bool Matches(string token, LineContent lineContent)
        {
            switch (lineContent)
            {
                case LineContent.OPTION:
                    return Regex.IsMatch(token, LINE_REGEX_OPTION);
                case LineContent.SECTION_HEADER:
                    return Regex.IsMatch(token, LINE_REGEX_SECTION);
                default:
                    throw new ArgumentException("Unknown enum value: " + lineContent.ToString());
            }
        }

        /// <summary>
        /// Removes all leading/trailing whitespaces from a config token. See:
        /// TrimLeadingWhitespaces
        /// TrimTrailingWhitespaces
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim whitespaces from.</param>
        protected static string TrimWhitespaces(string token)
        {
            return TrimLeadingWhitespaces(TrimTrailingWhitespaces(token));
        }

        /// <summary>
        /// Removes all leading whitespaces from a config token, as per the specification - stops if a whitespace prefixed
        /// with a slash is encountered.
        /// </summary>
        /// <returns></returns>
        /// <param name="token">The token to trim leading whitespaces from.</param>
        protected static string TrimLeadingWhitespaces(string token)
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
        protected static string TrimTrailingWhitespaces(string token)
        {
            Match match = Regex.Match(token, FILTER_TRAILING_WHITESPACES_REGEX);
            return match.Success ? match.Groups[1].Value : token;
        }

        /// <summary>
        /// Extracts trailing commentary from the specified line into the specified output
        /// parameter, and returns the line with the commentary removed.
        /// </summary>
        protected static string ExtractTrailingCommentary(string line, out string commentary)
        {
            int commentaryStartIndex = line.IndexOf(COMMENTARY_STARTER);
            if(commentaryStartIndex == -1)
            {
                commentary = null;
                return line;
            }
            else
            {
                commentary = line.Substring(commentaryStartIndex, line.Length - commentaryStartIndex);
                return line.Substring(0, commentaryStartIndex);
            }
        }

        #endregion
    }
}
