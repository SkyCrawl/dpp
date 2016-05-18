using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;

namespace Ini.Util
{
    /// <summary>
    /// Implementation of an observable list, as denoted by <see cref="INotifyCollectionChanged"/>.
    /// </summary>
    public class ObservableList<TValue> : IList<TValue>, INotifyCollectionChanged
    {
        #region Fields

        /// <summary>
        /// The inner list.
        /// </summary>
        protected List<TValue> list;

        #endregion

        #region INotifyCollectionChanged implementation

        /// <summary>
        /// The delegate to receive information about changes to this collection.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Ini.Util.ObservableList{TValue}"/> class.
        /// </summary>
        /// <param name="eventHandler">Event handler.</param>
        public ObservableList(NotifyCollectionChangedEventHandler eventHandler = null)
        {
            this.list = new List<TValue>();
            this.CollectionChanged = eventHandler;
        }

        #endregion

        #region Public Methods

        /// <Docs>The items to add to the current collection.</Docs>
        /// <para>Adds multiple items to the current collection.</para>
        /// <summary>
        /// Add the specified items to the collection.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
        public void AddRange(IEnumerable<TValue> items)
        {
            CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<TValue>(items)));
            list.AddRange(items);
        }

        #endregion

        #region IList implementation

        /// <Docs>To be added.</Docs>
        /// <para>Determines the index of a specific item in the current instance.</para>
        /// <summary>
        /// Returns zero-based index of the first occurrence of the specified item in the collection.
        /// </summary>
        /// <returns>The index.</returns>
        /// <param name="item">The item to search for.</param>
        public int IndexOf(TValue item)
        {
            return list.IndexOf(item);
        }

        /// <summary>
        /// Insert the specified item, at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, TValue item)
        {
            CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            list.Insert(index, item);
        }

        /// <summary>
        /// Removes item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, list[index]));
            list.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public TValue this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, list[index]));
                list[index] = value;
            }
        }

        #endregion

        #region ICollection implementation

        /// <Docs>The item to add to the current collection.</Docs>
        /// <para>Adds an item to the current collection.</para>
        /// <summary>
        /// Add the specified item to the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
        public void Add(TValue item)
        {
            CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            list.Add(item);
        }

        /// <summary>
        /// Clear this instance.
        /// </summary>
        public void Clear()
        {
            CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            list.Clear();
        }

        /// <Docs>The object to locate in the current collection.</Docs>
        /// <para>Determines whether the current collection contains a specific value.</para>
        /// <summary>
        /// Determines whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public bool Contains(TValue item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Copy items of this collection into the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">The index to start at.</param>
        public void CopyTo(TValue[] array, int arrayIndex)
        {
            foreach(TValue value in list)
            {
                array[arrayIndex] = value;
                arrayIndex++;
            }
        }

        /// <Docs>The item to remove from the current collection.</Docs>
        /// <para>Removes the first occurrence of an item from the current collection.</para>
        /// <summary>
        /// Removes the first occurrence of an item from the collection.
        /// </summary>
        /// <param name="item">The item.</param>
        public bool Remove(TValue item)
        {
            if(list.Contains(item))
            {
                CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            }
            return list.Remove(item);
        }

        /// <summary>
        /// Gets the count of items in this collection.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                return list.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this collection is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            return ((List<TValue>) list).GetEnumerator();
        }

        #endregion
    }
}
