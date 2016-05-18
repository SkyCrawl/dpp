using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ini.EventLoggers;
using Ini.Configuration;
using Ini.Specification;
using Ini.Util;
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
        /// Event logger to call when reading the configuration.
        /// </summary>
        protected IConfigReaderEventLogger eventLogger;

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
                this.Line = IniSyntax.ExtractAndRemoveCommentary(line, out trailingCommentary);
                this.TrailingCommentary = trailingCommentary;
                this.IsSection = IniSyntax.LineMatches(Line, Ini.IniSyntax.LineContent.SECTION_HEADER);
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

        #endregion

        #region IConfigParser implementation

        /// <summary>
        /// Passes essential objects for the next parsing task.
        /// </summary>
        /// <param name="specification">Specificaton for the result configuration.</param>
        /// <param name="eventLogger">Configuration reader event logger.</param>
        public void Prepare(ConfigSpec specification, IConfigReaderEventLogger eventLogger)
        {
            // reset the parser
            Reset();

            // prepare fields (forward arguments)
            this.specification = specification;
            this.config = new Config(specification);
            this.eventLogger = eventLogger;
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
            eventLogger.NewConfig(
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
            linkResolver.ResolveLinks(config, eventLogger);

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
            this.eventLogger = null;
        }

        #endregion

        #region Lower-level parsing methods

        /// <summary>
        /// Check preconditions for parsing.
        /// <exception cref="UndefinedSpecException">If validation mode is strict and no specification is specified.</exception>
        /// <exception cref="InvalidSpecException">If validation mode is strict and the specified specification is not valid.</exception>
        /// </summary>
        protected void PreconditionsTrue()
        {
            // no need to check specification for relaxed mode (everything will simply be a string)
            if(validationMode == ConfigValidationMode.Strict)
            {
                // we need a defined and valid specification
                config.ThrowIfSpecUndefinedOrInvalid(
                    eventLogger.SpecValidatiorLogger,
                    () => eventLogger.NoSpecification(),
                    () => eventLogger.InvalidSpecification()
                );
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
                if(string.IsNullOrEmpty(lineInfo.Line)) // this assumes that LineInfo trims whitespaces by itself
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
                    else if(IniSyntax.LineMatches(lineInfo.Line, Ini.IniSyntax.LineContent.OPTION))
                    {
                        ParseOption(context, lineInfo);
                    }
                    else
                    {
                        // report unknown syntax
                        eventLogger.UnknownLineSyntax(context.LineNumber, lineInfo.Line);

                        // and only keep the line's commentary
                        config.Add(new Commentary(new string[] { string.IsNullOrEmpty(lineInfo.TrailingCommentary) ? "" : lineInfo.TrailingCommentary }));
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
            string identifier = IniSyntax.ExtractSectionId(lineInfo.Line);
            SectionSpec sectionSpec = validationMode == ConfigValidationMode.Strict ? specification.GetSection(identifier) : null;
            if(string.IsNullOrEmpty(identifier))
            {
                throw new InvalidOperationException("Library specification states that identifiers must not be empty. " +
                    "Have you edited the respective regular expressions that demand it?");
            }
            else if(config.Contains(identifier))
            {
                // report error and skip until next section
                eventLogger.DuplicateSection(context.LineNumber, identifier);
                context.Mode = ParserMode.SKIP_UNTIL_NEXT_SECTION;
            }
            else if((validationMode == ConfigValidationMode.Strict) && (sectionSpec == null))
            {
                // report error and skip until next section
                eventLogger.NoSectionSpecification(context.LineNumber, identifier);
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
            // extract components, throwing exceptions if the syntax doesn't match
            string identifier, valueSeparator, value;
            IniSyntax.ExtractOptionComponents(lineInfo.Line, out identifier, out valueSeparator, out value);

            // check subsequent preconditions
            if(context.CurrentSection.Contains(identifier))
            {
                eventLogger.DuplicateOption(context.LineNumber, context.CurrentSection.Identifier, identifier);
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
                        eventLogger.NoOptionSpecification(
                            context.LineNumber,
                            context.CurrentSection.Identifier,
                            identifier);
                        return; // TODO: do we need to implement an error count in event loggers?
                    }
                    else
                    {
                        optionType = optionSpec.GetValueType();
                    }
                }

                // now we can create and register a new option, and parse the value
                Option newOption = new Option(identifier, optionType, lineInfo.TrailingCommentary);
                context.CurrentSection.Add(newOption);

                // and parse all elements of the option
                foreach(string element in IniSyntax.ExtractElements(value, valueSeparator))
                {
                    ParseElements(context, lineInfo, newOption, element);
                }
            }
        }

        /// <summary>
        /// Parse the given option's value, identify links and create/register value stubs.
        /// </summary>
        /// <param name="context">Parsing context.</param>
        /// <param name="lineInfo">Information about the current line.</param>
        /// <param name="option">The option being parsed.</param>
        /// <param name="element">The captured option value.</param>
        protected void ParseElements(ParserContext context, LineInfo lineInfo, Option option, string element)
        {
            if(IniSyntax.IsElementALink(element))
            {
                string[] components = IniSyntax.ExtractLinkComponents(element);
                if(components.Length == 2)
                {
                    // one '#' to denote section and option identifier... yay!
                    string targetSectionId = IniSyntax.TrimWhitespaces(components[0]);
                    string targetOptionId = IniSyntax.TrimWhitespaces(components[1]);

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
                        eventLogger.InvalidLinkTarget(
                            context.LineNumber,
                            context.CurrentSection.Identifier,
                            option.Identifier,
                            link);
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
                            uncertainLinks.Add(node, () => eventLogger.InvalidLinkTarget(
                                context.LineNumber,
                                context.CurrentSection.Identifier,
                                option.Identifier,
                                link));
                        }
                    }
                }
                else if(components.Length > 2) // too many target components
                {
                    // report the problem and don't try to interpret the link
                    eventLogger.ConfusingLinkTarget(
                        context.LineNumber,
                        context.CurrentSection.Identifier,
                        option.Identifier,
                        element);
                }
                else // not enough target components
                {
                    // report the problem and don't try to interpret the link
                    eventLogger.IncompleteLinkTarget(
                        context.LineNumber,
                        context.CurrentSection.Identifier,
                        option.Identifier,
                        element);
                }
            }
            else // we have a general string value
            {
                ValueStub newStub = new ValueStub(option.ValueType, element);

                // register it and notify derived classes
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
    }
}
