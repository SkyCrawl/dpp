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
using Ini.Configuration.Values.Links;
using Ini.Configuration.Base;

namespace Ini
{
    /// <summary>
    /// A one-time-use class to parse a configuration from text.
    /// </summary>
    public class ConfigParser : IConfigParser
    {
        #region Constants

        const string ESCAPE_SEQUENCE = "\\";

        const string IDENTIFIER_START_CHAR_REGEX = "[a-zA-Z.$:]";
        const string IDENTIFIER_SUFFIX_CHAR_REGEX = "[a-zA-Z0-9_~\\-.:$ " + ESCAPE_SEQUENCE + "]";
        const string IDENTIFIER_REGEX = IDENTIFIER_START_CHAR_REGEX + IDENTIFIER_SUFFIX_CHAR_REGEX + "*";

        const string LINE_REGEX_OPTION = "^(" + IDENTIFIER_REGEX + ")" + "(=(?::=)?)" + "(.*)$";
        const string LINE_REGEX_SECTION = "^\\[" + IDENTIFIER_REGEX + "\\]$";

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
        /// Preprocessed lines of input.
        /// </summary>
        protected List<LineInfo> lines;

        /// <summary>
        /// A wrapper for several state variables relevant for parsing methods.
        /// </summary>
        protected ParserContext context;

        /// <summary>
        /// Collection of links with unknown targets at the time of their parsing.
        /// </summary>
        protected Dictionary<LinkNode, Action> uncertainLinks;

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
                this.Line = TrimWhitespaces(RemoveTrailingCommentary(line, out trailingCommentary));
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
            /// Preprocessed lines to parse.
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

        #region IConfigParser implementation

        /// <summary>
        /// Passes essential objects for the next parsing task.
        /// </summary>
        /// <param name="specification">The result configuration's specification.</param>
        /// <param name="configEventLog">Configuration reading event logger.</param>
        /// <param name="specEventLog">Specification validation event logger.</param>
        public void Prepare(ConfigSpec specification, IConfigReaderEventLogger configEventLog, ISpecValidatorEventLogger specEventLog)
        {
            // reset the parser
            Reset();

            // prepare fields (forward arguments)
            this.specification = specification;
            this.configEventLog = configEventLog;
            this.specEventLog = specEventLog;
        }

        /// <summary>
        /// Perform the next parsing task.
        /// </summary>
        /// <param name="input">The input for parsing.</param>
        /// <param name="validationMode">The validation mode to use.</param>
        /// <exception cref="UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
        /// <exception cref="InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
        /// <exception cref="MalformedConfigException">If the configuration's format is malformed.</exception>
        public Config Parse(TextReader input, ConfigValidationMode validationMode)
        {
            // prepare fields (forward arguments)
            this.validationMode = validationMode;

            // report before parsing
            configEventLog.NewConfig(
                config.Origin,
                config.Spec != null ? config.Spec.Origin : null,
                validationMode);

            // parse
            PreconditionsTrue();
            PreprocessLines(input);
            ParseLines();

            // now that the configuration is fully parsed, check uncertain links again
            foreach(KeyValuePair<LinkNode, Action> entry in uncertainLinks)
            {
                // if the link's target still hasn't been defined...
                if(config.GetOption(entry.Key.Target.Section, entry.Key.Target.Option) == null)
                {
                    // report
                    entry.Value.Invoke();

                    // remove such link from the configuration and dependency graph
                    Option option = config.GetOption(entry.Key.Origin.Section, entry.Key.Origin.Option);
                    option.Elements.Remove(entry.Key.LinkElement);
                    linkResolver.RemoveLink(entry.Key);

                    // empty option value is explicitly allowed, so there's no real need to remove the option
                }
            }

            // once we have made sure the configuration contains only the valid stuff, resolve all links
            linkResolver.ResolveLinks(config, configEventLog);

            // interpret option values in their correct contexts
            foreach(Section section in config.GetAllSections())
            {
                foreach(Option option in section.GetAllOptions())
                {
                    // interpret the values
                    List<IElement> interpreted = new List<IElement>();
                    foreach(IElement elem in option)
                    {
                        if(elem is IValue)
                        {
                            interpreted.Add((elem as ValueStub).InterpretSelf());
                        }
                        else if(elem is ILink)
                        {
                            (elem as ILink).InterpretSelf();
                            interpreted.Add(elem);
                        }
                        else
                        {
                            throw new InvalidOperationException("Unknown value type: " + elem.GetType().ToString());
                        }
                    }

                    // and replace them for the stubs
                    option.Elements.Clear();
                    option.Elements.AddRange(interpreted);
                }
            }

            // and finally, return
            return config;
        }

        #endregion

        #region Resetting the parser

        /// <summary>
        /// Reset this instance.
        /// </summary>
        protected void Reset()
        {
            this.specification = null;
            this.config = null;
            this.linkResolver = new LinkResolver();
            this.lines = new List<LineInfo>();
            this.context = null;
            this.uncertainLinks = new Dictionary<LinkNode, Action>();
            this.validationMode = ConfigValidationMode.Strict;
            this.specEventLog = null;
            this.configEventLog = null;
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
        /// Reads the specified input into <see cref="lines"/> and creates <see cref="context"/>.
        /// </summary>
        /// <param name="input">The input.</param>
        protected void PreprocessLines(TextReader input)
        {
            string line;
            while((line = input.ReadLine()) != null)
            {
                lines.Add(new LineInfo(line));
            }
            context = new ParserContext(lines);
        }

        /// <summary>
        /// Parse the whole input into <see cref="config"/> while also filling up
        /// <see cref="linkResolver"/>.
        /// </summary>
        protected void ParseLines()
        {
            List<string> filler = new List<string>();

            // let the context choose the right lines to parse
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
                        // must save into the appropriate context
                        if(context.CurrentSection != null)
                        {
                            context.CurrentSection.Add(new Commentary(filler));
                        }
                        else
                        {
                            config.Add(new Commentary(filler));
                        }

                        // filler has been saved, so clear
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
            SectionSpec sectionSpec = specification.GetSection(identifier);
            if(string.IsNullOrEmpty(identifier))
            {
                throw new InvalidOperationException("Library specification states that identifiers must not be empty. " +
                    "Have you edited the respective regular expressions that demand it?");
            }
            else if(config.Contains(identifier))
            {
                // report error and skip until next section
                configEventLog.DuplicateSection(context.LineNumber, identifier);
                context.Mode = ParserMode.SKIP_UNTIL_NEXT_SECTION;
            }
            else if((validationMode == ConfigValidationMode.Strict) && (sectionSpec == null))
            {
                // report error and skip until next section
                configEventLog.NoSectionSpecification(context.LineNumber, identifier);
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
                string identifier = TrimTrailingWhitespaces(match.Groups[1].Value); // the specification tells us to do so
                string separator = match.Groups[2].Value;
                string value = match.Groups[3].Value; // must not explicitly trim leading whitespaces of a value

                // check preconditions
                if(string.IsNullOrEmpty(identifier))
                {
                    throw new InvalidOperationException("Library specification states that identifiers must not be empty. " +
                        "Have you edited the respective regular expressions that demand it?");
                }
                else if(context.CurrentSection.Contains(identifier))
                {
                    configEventLog.DuplicateOption(context.LineNumber, context.CurrentSection.Identifier, identifier);
                }
                else
                {
                    // check against the specification and determine option type
                    Type optionType = typeof(string); // the one and only type for relaxed mode
                    if(validationMode == ConfigValidationMode.Strict)
                    {
                        OptionSpec optionSpec = specification.GetOption(context.CurrentSection.Identifier, identifier);
                        if(optionSpec == null)
                        {
                            // report error and skip further parsing (return)
                            configEventLog.NoOptionSpecification(
                                context.LineNumber,
                                context.CurrentSection.Identifier,
                                identifier);
                            return;
                        }
                        else
                        {
                            optionType = optionSpec.GetValueType();
                        }
                    }

                    // now we can create and register a new option, and parse the value
                    Option newOption = new Option(identifier, optionType, lineInfo.TrailingCommentary);
                    context.CurrentSection.Add(newOption);

                    // determine the record separator to use
                    string recordSeparator = separator.Contains(':') ? ":" : ",";

                    // iterate through all occurrences of the record separator (if unescaped)
                    int recordStartIndex = 0;
                    foreach(Match recordMatch in Regex.Matches(value, UnescapedTokenRegex(recordSeparator)))
                    {
                        // no need to check success or failure, simply parse
                        ParseOptionRecord(context, lineInfo, newOption, value.Substring(recordStartIndex, recordMatch.Index));
                        recordStartIndex = recordMatch.Index;
                    }

                    // and don't forget to parse the remaining record (or default)
                    ParseOptionRecord(context, lineInfo, newOption, value.Substring(recordStartIndex, value.Length - recordStartIndex));
                }
            }
            else
            {
                throw new InvalidOperationException("This method can not be called if the line doesn't match the regular expression for options.");
            }
        }

        /// <summary>
        /// Parse the given option value, identify links and create/register value stubs.
        /// </summary>
        /// <param name="context">Parsing context.</param>
        /// <param name="lineInfo">Information about the current line.</param>
        /// <param name="option">The option being parsed.</param>
        /// <param name="value">The captured option value.</param>
        protected void ParseOptionRecord(ParserContext context, LineInfo lineInfo, Option option, string value)
        {
            // check the link syntax (content must not be empty thanks to identifiers that must not be empty as well)
            Match match = Regex.Match(TrimWhitespaces(value), UnescapedTokenRegex("^\\${([^}]+)}$"));
            if(match.Success)
            {
                // use the first capture group to fetch the link's content
                string[] splitted = match.Groups[1].Value.Split('#');
                if(splitted.Length == 2)
                {
                    // one '#' to denote section and option identifier... yay!
                    string targetSectionId = TrimWhitespaces(splitted[0]);
                    string targetOptionId = TrimWhitespaces(splitted[1]);

                    // create the link
                    LinkOrigin linkOrigin = new LinkOrigin(context.CurrentSection.Identifier, option.Identifier);
                    LinkTarget linkTarget = new LinkTarget(targetSectionId, targetOptionId);
                    InclusionLink link = new InclusionLink(option.ValueType, linkTarget);
                    LinkNode node = new LinkNode(link, linkOrigin);

                    // check preconditions
                    if(!linkOrigin.IsValidKeySource())
                    {
                        throw new InvalidOperationException("Library specification states that identifiers must not be empty. " +
                            "Have you edited the respective regular expressions that demand it?");
                    }
                    else if(!linkTarget.IsValidKeySource())
                    {
                        configEventLog.InvalidLinkTarget(
                            context.LineNumber,
                            context.CurrentSection.Identifier,
                            option.Identifier,
                            value);
                    }
                    else
                    {
                        // register the link
                        option.Elements.Add(link);
                        linkResolver.AddLink(node);

                        // and determine whether the link's target is known yet
                        if(config.GetOption(targetSectionId, targetOptionId) == null)
                        {
                            // when the parsing is finished, we will check again
                            uncertainLinks.Add(node, () => configEventLog.InvalidLinkTarget(
                                context.LineNumber,
                                context.CurrentSection.Identifier,
                                option.Identifier,
                                value));
                        }
                    }
                }
                else if(splitted.Length > 2) // too many target components
                {
                    // report the problem and don't try to interpret the link
                    configEventLog.ConfusingLinkTarget(
                        context.LineNumber,
                        context.CurrentSection.Identifier,
                        option.Identifier,
                        value);
                }
                else // not enough target components
                {
                    // report the problem and don't try to interpret the link
                    configEventLog.IncompleteLinkTarget(
                        context.LineNumber,
                        context.CurrentSection.Identifier,
                        option.Identifier,
                        value);
                }
            }
            else // we have a general string value
            {
                ValueStub newStub = new ValueStub(option.ValueType, value);

                // register it and notify derrived classes
                option.Elements.Add(newStub);
                OnValueStubRegistered(newStub);
            }
        }

        #endregion

        #region Special callbacks

        /// <summary>
        /// A callback when a value stub is created. Override to execute custom alterations.
        /// </summary>
        /// <param name="stub">Stub.</param>
        protected virtual void OnValueStubRegistered(ValueStub stub)
        {
        }

        #endregion

        #region Static helpers

        /// <summary>
        /// Builds a regular expression that matches the specified token as long as it
        /// is not preceded by an escape sequence (<see cref="ESCAPE_SEQUENCE"/>).
        /// </summary>
        /// <returns>The regular expression.</returns>
        /// <param name="token">The token.</param>
        protected static string UnescapedTokenRegex(string token)
        {
            return "(?<!" + ESCAPE_SEQUENCE + ")" + token;
        }

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
        protected static string TrimTrailingWhitespaces(string token)
        {
            // use the first capture group to match the filtered string token
            // meaning: trailing whitespaces are ignored, up until the last one that is escaped

            Match match = Regex.Match(token, "^(.*?(?:" + ESCAPE_SEQUENCE + "\\s)?)\\s*$");
            return match.Success ? match.Groups[1].Value : token;
        }

        /// <summary>
        /// Extracts trailing commentary from the specified line into the specified output
        /// parameter, and returns the line with the commentary removed.
        /// </summary>
        protected static string RemoveTrailingCommentary(string line, out string commentary)
        {
            // match the first semicolon that is not preceded by an escape sequence
            Match match = Regex.Match(line, UnescapedTokenRegex(";"));
            if(match.Success)
            {
                commentary = line.Substring(match.Index, line.Length - match.Index);
                return line.Substring(0, match.Index);
            }
            else
            {
                commentary = null;
                return line;
            }
        }

        #endregion
    }
}
