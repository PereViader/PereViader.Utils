using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Collections
{
    //TODO: Implement a TypeDictionary<T> and use it in DynamicDispatch to optimize GC

    /// <summary>
    /// A generic dictionary where the key is a Type object and the value is an object.
    /// Useful for doing type safe conversion between non generic type handling code and type safe code
    /// </summary>
    public sealed class TypeDictionary : Dictionary<Type, object>
    {
        /// <summary>
        /// Adds a new element with the provided value to the dictionary.
        /// The type of the provided value is used as a key.
        /// </summary>
        /// <typeparam name="T">The type of the value to be added.</typeparam>
        /// <param name="value">The value to add to the dictionary.</param>
        public void Add<T>(T value)
        {
            var type = typeof(T);

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            Add(type, value);
        }

        /// <summary>
        /// Retrieves the value associated with the given type key.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <returns>The value associated with the given type key.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the key is not found in the dictionary.</exception>
        public T GetValue<T>()
        {
            if (!TryGetValue<T>(out T? value))
            {
                throw new KeyNotFoundException($"The key {typeof(T).FullName} was not found in the TypeDictionary");
            }

            return value!;
        }

        /// <summary>
        /// Tries to get the value associated with the specified type key.
        /// </summary>
        /// <typeparam name="T">The type of the value to get.</typeparam>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found;
        /// otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the TypeDictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue<T>(out T? value)
        {
            var type = typeof(T);
            
            if (!TryGetValue(type, out object? objectValue))
            {
                value = default;
                return false;
            }

            value = (T)objectValue;
            return true;
        }
        
        public bool TryGetValue<TKey, TValue>(out TValue? value)
        {
            var type = typeof(TKey);
            
            if (!TryGetValue(type, out var objectValue))
            {
                value = default;
                return false;
            }

            value = (TValue)objectValue;
            return true;
        }
        
        public bool TryGetValue<T>(
            bool checkAssignableTypes,
            out T value)
        {
            var result = TryGetValue(typeof(T), checkAssignableTypes, out var objectValue);
            value = (T)objectValue;
            return result;
        }
        
        public bool TryGetValue<TKey, TValue>(
            bool checkAssignableTypes,
            out TValue value)
        {
            var result = TryGetValue(typeof(TKey), checkAssignableTypes, out var objectValue);
            value = (TValue)objectValue;
            return result;
        }
        
        public bool TryGetValue(
            Type type,
            bool checkAssignableTypes,
            out object value)
        {
            if (TryGetValue(type, out value))
            {
                return true;
            }

            if (!checkAssignableTypes || !TryGetAssignableKeyType(type, out var assignableKeyType))
            {
                return false;
            }

            return TryGetValue(assignableKeyType, out value);
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

        bool TryGetAssignableKeyType(Type type, out Type assignableKeyType)
        {
            foreach (var key in Keys)
            {
                if (key.IsAssignableFrom(type))
                {
                    assignableKeyType = key;
                    return true;
                }
            }

            assignableKeyType = default!;
            return false;
        }
    }
}