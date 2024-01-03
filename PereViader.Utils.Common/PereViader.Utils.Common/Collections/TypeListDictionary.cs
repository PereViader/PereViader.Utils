using System;
using System.Collections.Generic;
using PereViader.Utils.Common.Attributes;

namespace PereViader.Utils.Common.Collections
{
    /// <summary>
    /// A generic dictionary where the key is a Type object and the value is List of objects of that type.
    /// Useful for doing type safe conversion between non generic type handling code and type safe code
    /// </summary>
    [Experimental]
    public sealed class TypeListDictionary : Dictionary<Type, object>
    {
        /// <summary>
        /// Adds a new element with the provided value to the dictionary.
        /// The type of the provided value is used as a key.
        /// </summary>
        /// <typeparam name="T">The type of the value to be added.</typeparam>
        /// <param name="value">The value to add to the dictionary.</param>
        public void Add<T>(T value)
        {
            if (!TryGetValuesInternal<T>(out var list))
            {
                list = new List<T>();
                this[typeof(T)] = list;
            }

            list!.Add(value);
        }

        /// <summary>
        /// Removes an element with the provided value from the dictionary.
        /// The type of the provided value is used as a key.
        /// </summary>
        /// <typeparam name="T">The type of the value to be removed.</typeparam>
        /// <param name="value">The value to remove from the dictionary.</param>
        public bool Remove<T>(T value)
        {
            if (!TryGetValuesInternal<T>(out var list))
            {
                return false;
            }

            var removed = list!.Remove(value);
            
            if (list.Count == 0)
            {
                Remove(typeof(T));
            }

            return removed;
        }

        /// <summary>
        /// Retrieves the value associated with the given type key.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <returns>The value associated with the given type key.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the key is not found in the dictionary.</exception>
        public IReadOnlyList<T> GetValues<T>()
        {
            if (!TryGetValues<T>(out var value))
            {
                throw new KeyNotFoundException($"The key {typeof(T).FullName} was not found in the TypeDictionary");
            }

            return value!;
        }

        /// <summary>
        /// Tries to get the value associated with the specified type key.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="values">When this method returns, contains the value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the TypeDictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValues<T>(out IReadOnlyList<T>? values)
        {
            if (!TryGetValuesInternal<T>(out var listValues))
            {
                values = default;
                return false;
            }

            values = listValues;
            return true;
        }

        bool TryGetValuesInternal<T>(out List<T>? values)
        {
            var type = typeof(T);
            
            if (!TryGetValue(type, out object? objectList))
            {
                values = default;
                return false;
            }

            values = (List<T>)objectList;
            return true;
        }

        /// <summary>
        /// Determines whether the TypeDictionary contains the specified type key.
        /// </summary>
        /// <typeparam name="T">The type of the key to locate in the TypeDictionary.</typeparam>
        /// <returns>true if the TypeDictionary contains an element with the specified key; otherwise, false.</returns>
        public bool ContainsKey<T>()
        {
            var type = typeof(T);
            return ContainsKey(type);
        }
    }
}