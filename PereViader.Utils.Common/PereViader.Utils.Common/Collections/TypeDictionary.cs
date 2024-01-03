using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Collections
{
    public sealed class TypeDictionary : Dictionary<Type, object>
    {
        public bool ContainsKey<T>() => ContainsKey(typeof(T));
        public void Add<T>(T value) => Add(typeof(T), value!);
        
        public bool TryGetValue<T>(out T value)
        {
            if (!TryGetValue(typeof(T), out var objectValue))
            {
                value = default!;
                return false;
            }

            value = (T)objectValue;
            return true;
        }

        public bool TryGetValue<T>(
            bool checkAssignableTypes,
            out T value)
        {
            if (!TryGetValue(typeof(T), checkAssignableTypes, out var objectValue))
            {
                value = default!;
                return false;
            }
            
            value = (T)objectValue;
            return true;
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

            if (!checkAssignableTypes || !TryGetContainedAssignableType(type, out var assignableKeyType))
            {
                return false;
            }

            return TryGetValue(assignableKeyType, out value);
        }

        public bool TryGetContainedAssignableType(Type type, out Type assignableKeyType)
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
    
    public sealed class TypeDictionary<TValue> : Dictionary<Type, TValue>
    {
        public bool ContainsKey<TKey>() => ContainsKey(typeof(TKey));
        public void Add<TKey>(TValue value) => Add(typeof(TKey), value);
        public bool TryGetValue<TKey>(out TValue value) => TryGetValue(typeof(TKey), out value);
        public bool TryGetValue<TKey>(bool checkAssignableTypes, out TValue value)
            => TryGetValue(typeof(TKey), checkAssignableTypes, out value);

        public bool TryGetValue(
            Type type,
            bool checkAssignableTypes,
            out TValue value)
        {
            if (TryGetValue(type, out value))
            {
                return true;
            }

            if (!checkAssignableTypes || !TryGetContainedAssignableType(type, out var assignableKeyType))
            {
                return false;
            }

            return TryGetValue(assignableKeyType, out value);
        }
        
        public bool TryGetContainedAssignableType(Type type, out Type assignableKeyType)
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