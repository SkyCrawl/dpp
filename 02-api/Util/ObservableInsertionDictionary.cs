using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Ini.Util
{
    /// <summary>
    /// Dictionary storing key-value pairs while keeping insertion order. Also implements the
    /// <see cref="INotifyCollectionChanged"/> to keep track of changes to the collection.
    /// </summary>
    public class ObservableInsertionDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>, INotifyCollectionChanged
    {
        #region Fields

        /// <summary>
        /// The inner ordered dictionary.
        /// </summary>
        protected OrderedDictionary dictionary;

        #endregion

        #region INotifyCollectionChanged implementation

        /// <summary>
        /// The delegate to receive information about changes to this collection.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Util.ObservableInsertionDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="eventHandler">Event handler.</param>
        public ObservableInsertionDictionary(NotifyCollectionChangedEventHandler eventHandler = null)
        {
            this.dictionary = new OrderedDictionary();
            this.CollectionChanged = eventHandler;
        }

        #endregion

        #region IDictionary implementation

        /// <summary>
        /// Add the specified identifier and value to the collection.
        /// </summary>
        /// <exception cref="System.ArgumentException">An item with the same key has already been added.</exception>
        /// <param name="identifier">Identifier.</param>
        /// <param name="value">Value.</param>
        public void Add(TKey identifier, TValue value)
        {
            CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(identifier, value)));
            dictionary.Add(identifier, value);
        }

        /// <summary>
        /// Determines whether the collection contains an entry with the specified identifier.
        /// </summary>
        /// <returns><c>true</c>, if identifier is containted, <c>false</c> otherwise.</returns>
        /// <param name="identifier">Identifier.</param>
        public bool ContainsKey(TKey identifier)
        {
            return dictionary.Contains(identifier);
        }
            
        /// <summary>
        /// Removes the entry with the specified identifier from the collection.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        public bool Remove(TKey identifier)
        {
            bool contains = dictionary.Contains(identifier);
            if(contains)
            {
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(identifier, (TValue) dictionary[identifier])));
                dictionary.Remove(identifier);
            }
            return contains;
        }
            
        /// <summary>
        /// Outputs the entry with the specified identifier and returns true if found.
        /// </summary>
        /// <returns><c>true</c>, if value was found, <c>false</c> otherwise.</returns>
        /// <param name="identifier">Identifier.</param>
        /// <param name="value">Value.</param>
        public bool TryGetValue(TKey identifier, out TValue value)
        {
            value = ContainsKey(identifier) ? (TValue) dictionary[identifier] : default(TValue);
            if(typeof(TValue).IsValueType)
            {
                return (object) value != Activator.CreateInstance(typeof(TValue));
            }
            else
            {
                return value != null;
            }

        }

        /// <summary>
        /// Gets or sets the entry with the specified identifier.
        /// </summary>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">Entry not found in the collection.</exception>
        public TValue this[TKey identifier]
        {
            get
            {
                if(ContainsKey(identifier))
                {
                    return (TValue) dictionary[identifier];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            set
            {
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(
                    ContainsKey(identifier) ? NotifyCollectionChangedAction.Replace : NotifyCollectionChangedAction.Add,
                    new KeyValuePair<TKey, TValue>(identifier, value)));
                dictionary[identifier] = value;
            }
        }
    
        /// <summary>
        /// Gets all identifiers.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                // the inner dictionary is not generic so we have to explicitly retype the value collection
                List<TKey> result = new List<TKey>();
                foreach(object value in dictionary.Keys)
                {
                    result.Add((TKey) value);
                }
                return result;
            }
        }

        /// <summary>
        /// Gets all values.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                // the inner dictionary is not generic so we have to explicitly retype the value collection
                List<TValue> result = new List<TValue>();
                foreach(object value in dictionary.Values)
                {
                    result.Add((TValue) value);
                }
                return result;
            }
        }

        #endregion

        #region ICollection implementation

        /// <summary>
        /// Add the specified entry to the collection.
        /// </summary>
        /// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
        /// <param name="entry">Entry.</param>
        public void Add(KeyValuePair<TKey, TValue> entry)
        {
            Add(entry.Key, entry.Value);
        }

        /// <summary>
        /// Clear the collection.
        /// </summary>
        public void Clear()
        {
            CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            dictionary.Clear();
        }
            
        /// <summary>
        /// Determines whether the collection contains the specified entry.
        /// </summary>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        /// <param name="entry">Entry.</param>
        public bool Contains(KeyValuePair<TKey, TValue> entry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copies the collection's content into the specified array, starting at the specified
        /// index.
        /// </summary>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        /// <param name="array">Array.</param>
        /// <param name="index">Index.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            throw new NotImplementedException();
        }
            
        /// <summary>
        /// Removes the specified entry from the collection.
        /// </summary>
        /// <exception cref="System.NotImplementedException">Not implemented.</exception>
        /// <param name="entry">Entry.</param>
        public bool Remove(KeyValuePair<TKey, TValue> entry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Count of the collection's elements.
        /// </summary>
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly
        {
            get
            {
                return dictionary.IsReadOnly;
            }
        }

        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Gets the content enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        /// <summary>
        /// Gets the content enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>) dictionary).GetEnumerator();
        }

        #endregion
    }
}
