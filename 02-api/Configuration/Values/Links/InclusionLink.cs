using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Ini.Configuration;
using Ini.Configuration.Base;
using Ini.Specification;
using Ini.EventLoggers;
using Ini.Util.LinkResolving;
using Ini.Util;
using Ini.Configuration.Values;
using Ini.Exceptions;
using System.Collections;

namespace Ini.Configuration.Values.Links
{
    /// <summary>
    /// Class representing an inclusion link that can be added to an <see cref="Option"/>.
    /// </summary>
    public class InclusionLink : ILink
    {
        #region Properties

        /// <summary>
        /// Readonly type associated with the parent option.
        /// </summary>
        /// <value>The type of the value.</value>
        public Type ValueType { get; private set; }

        /// <summary>
        /// This link's target.
        /// </summary>
        /// <value>The target.</value>
        public LinkTarget Target { get; private set; }

        /// <summary>
        /// After the link is resolved, this collection contains the link's value objects.
        /// Their type must be identical to <see cref="ValueType"/>.
        /// </summary>
        /// <value>The values.</value>
        public ObservableList<IValue> Values { get; private set; }

        /// <summary>
        /// An indicator whether this link has been resolved.
        /// </summary>
        /// <value><c>true</c> if this link has been resolved; otherwise, <c>false</c>.</value>
        public bool IsResolved { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Configuration.Values.Links.InclusionLink"/> class.
        /// </summary>
        /// <param name="valueType">The value type associated with the parent option.</param>
        /// <param name="target">This link's target.</param>
        public InclusionLink(Type valueType, LinkTarget target)
        {
            this.ValueType = valueType;
            this.Target = target;
            this.Values = new ObservableList<IValue>(new NotifyCollectionChangedEventHandler(OnContentChanged));
            this.IsResolved = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Looks into <see cref="Target"/> and if it's resolved, updates the inner data accordingly.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <param name="configEventLogger">Logger to use if something goes wrong.</param>
        public void Resolve(Config config, IConfigReaderEventLogger configEventLogger)
        {
            if(IsResolved)
            {
                throw new InvalidOperationException("This link has already been resolved.");
            }
            else
            {
                Option targetOption = config.GetOption(Target.Section, Target.Option);
                this.Values.Clear();
                foreach(ValueStub value in targetOption.GetObjectValues())
                {
                    this.Values.Add(new ValueStub(ValueType, value.Value));
                }
                this.IsResolved = true;
            }
        }

        /// <summary>
        /// Interprets <see cref="Values"/> and converts instances of <see cref="ValueStub"/> to
        /// the respective value objects that inherit from <see cref="ValueBase{T}"/>.
        /// </summary>
        public void InterpretSelf()
        {
            IEnumerable<IValue> interpreted = Values.Select<IValue, IValue>(value => (value as ValueStub).InterpretSelf());
            Values.Clear();
            Values.AddRange(interpreted);
        }

        /// <summary>
        /// Determines whether the element conforms to the given option specification.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <param name="section">The current section.</param>
        /// <param name="specification">The current option's specification.</param>
        /// <param name="configLogger">Configuration validation event logger.</param>
        /// <returns></returns>
        public bool IsValid(Config config, string section, OptionSpec specification, IConfigValidatorEventLogger configLogger)
        {
            /*
             * First catch bugs.
             */

            // if there's a value type mismatch, this code should never have been called, so:
            if(!ValueType.Equals(specification.GetValueType()))
            {
                throw new InvalidOperationException("Value type is assumed to have been checked in the parent option and yet, there's a mismatch at value-level. A bug?");
            }

            if(!IsResolved)
            {
                throw new InvalidOperationException("Can not validate links that have not been resolved.");
            }

            /*
             * And then validate.
             */

            // prepare the result validation state
            bool linkValid = true;

            // validate target
            if(config.GetOption(Target.Section, Target.Option) == null)
            {
                linkValid = false;
                configLogger.LinkInconsistent(
                    section,
                    specification.Identifier,
                    this);
            }

            // validate the inner structure against the specification
            foreach(IValue value in Values)
            {
                if(!value.IsValid(config, section, specification, configLogger))
                {
                    linkValid = false;
                }
            }

            // and return
            return linkValid;
        }

        /// <summary>
        /// Serializes this element into a string that can be deserialized back using <see cref="ConfigParser"/>.
        /// </summary>
        /// <param name="config">The parent configuration.</param>
        /// <returns>The element converted to a string.</returns>
        public string ToOutputString(Config config)
        {
            if (IsTargetDifferentFromInnerValues(config))
            {
                return IniSyntax.SerializeLink(Target);
            }
            else
            {
                return IniSyntax.SerializeElements(Values, config);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Determines whether this link's values or the target option have changed
        /// since the parsing operation took place.
        /// </summary>
        /// <returns><c>true</c> if this link's values are still identical to the target option, assuming that
        /// the option has not been removed from the configuration; otherwise, <c>false</c>.</returns>
        /// <param name="config">The parent configuration.</param>
        protected bool IsTargetDifferentFromInnerValues(Config config)
        {
            // check whether the target has been removed from the configuration
            Option targetOption = config.GetOption(Target.Section, Target.Option);
            if(targetOption == null)
            {
                return true;
            }

            // check whether the target's values differ from this link's values
            IList<IValue> targetValues = targetOption.GetObjectValues();
            if(targetValues.Count != this.Values.Count)
            {
                return true;
            }
            else // check individual values pair-wise
            {
                IEnumerator<IValue> thisValue = Values.GetEnumerator();
                IEnumerator<IValue> targetValue = targetValues.GetEnumerator();
                while (targetValue.MoveNext() && thisValue.MoveNext())
                {
                    if(!targetValue.Current.GetValue<object>().Equals(thisValue.Current.GetValue<object>()))
                    {
                        return true;
                    }
                }
            }

            // if not, return false
            return false;
        }

        #endregion

        #region Keeping internal state

        /// <summary>
        /// The delegate for <see cref="INotifyCollectionChanged"/>.
        /// </summary>
        /// <param name="sender">The observed collection.</param>
        /// <param name="e">Changes that occurred.</param>
        protected void OnContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                    // check the invariants and throw an exception if required
                    foreach(IValue value in e.NewItems)
                    {
                        if(!value.ValueType.IsSubclassOf(ValueType))
                        {
                            throw new InvariantBrokenException(string.Format(
                                "Only elements with value type of '{0}' are allowed.",
                                ValueType.FullName));
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                    // one or more items were moved or removed - that doesn't break any invariant
                    break;

                default:
                    throw new ArgumentException("Unknown enum value: " + e.Action.ToString());
            }
        }

        #endregion
    }
}
