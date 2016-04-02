using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ini.Configuration;

namespace Ini.Util
{
	/// <summary>
	/// Dictionary storing blocks of configuration while keeping insertion order.
	/// </summary>
	public class ConfigBlockDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>> where TValue : ConfigBlockBase
	{
		/// <summary>
		/// The inner ordered dictionary.
		/// </summary>
		private OrderedDictionary Dictionary;

		/// <summary>
		/// Initializes a new instance of the <see cref="Ini.Util.ConfigBlockDictionary{TKey,TValue}"/> class.
		/// </summary>
		public ConfigBlockDictionary()
		{
			this.Dictionary = new OrderedDictionary();
		}

		#region IDictionary implementation

		/// <summary>
		/// Add the specified identifier and value to the collection.
		/// </summary>
		/// <exception cref="System.ArgumentException">An item with the same key has already been added.</exception>
		/// <param name="identifier">Identifier.</param>
		/// <param name="value">Value.</param>
		public void Add(TKey identifier, TValue value)
		{
			Dictionary.Add(identifier, value);
		}

		/// <summary>
		/// Determines whether the collection contains an entry with the specified identifier.
		/// </summary>
		/// <returns><c>true</c>, if identifier is containted, <c>false</c> otherwise.</returns>
		/// <param name="identifier">Identifier.</param>
		public bool ContainsKey(TKey identifier)
		{
			return Dictionary.Contains(identifier);
		}
			
		/// <summary>
		/// Removes the entry with the specified identifier from the collection.
		/// </summary>
		/// <param name="identifier">Identifier.</param>
		public bool Remove(TKey identifier)
		{
			bool contains = Dictionary.Contains(identifier);
			if(contains)
			{
				Dictionary.Remove(identifier);
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
			value = ContainsKey(identifier) ? (TValue) Dictionary[identifier] : default(TValue);
			return value != default(TValue);
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
					return (TValue) Dictionary[identifier];
				}
				else
				{
					throw new KeyNotFoundException();
				}
			}
			set
			{
				Dictionary[identifier] = value;
			}
		}
	
		/// <summary>
		/// Gets all identifiers.
		/// </summary>
		public ICollection<TKey> Keys
		{
			get
			{
				return (ICollection<TKey>) Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets all values.
		/// </summary>
		public ICollection<TValue> Values
		{
			get
			{
				return (ICollection<TValue>) Dictionary.Values;
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
			Dictionary.Add(entry.Key, entry.Value);
		}

		/// <summary>
		/// Clear the collection.
		/// </summary>
		public void Clear()
		{
			Dictionary.Clear();
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
				return Dictionary.Count;
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
				return Dictionary.IsReadOnly;
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
			return Dictionary.GetEnumerator();
		}

		/// <summary>
		/// Gets the content enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>) Dictionary).GetEnumerator();
		}

		#endregion
	}
}
