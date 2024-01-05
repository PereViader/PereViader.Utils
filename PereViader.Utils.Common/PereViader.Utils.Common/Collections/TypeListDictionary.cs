using System;
using System.Collections.Generic;
using PereViader.Utils.Common.Attributes;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.Collections
{
    /// <summary>
    /// A generic dictionary where the key is a Type object and the value is List of objects of that type.
    /// Useful for doing type safe conversion between non generic type handling code and type safe code
    /// </summary>
    [Experimental]
    public sealed class TypeListDictionary : Dictionary<Type, object>
    {
        public bool ContainsKey<T>() => ContainsKey(typeof(T));
        
        public void AddValue<T>(T value)
        {
            if (!TryGetValues<T>(out var list))
            {
                list = new List<T>();
                this[typeof(T)] = list;
            }

            list.Add(value);
        }
        
        public bool Remove<T>(T value)
        {
            if (!TryGetValues<T>(out var list))
            {
                return false;
            }

            var removed = list.Remove(value);
            
            if (list.Count == 0)
            {
                Remove(typeof(T));
            }

            return removed;
        }

        public List<T>? GetValues<T>() => TryGetValues<T>(out var values).ToNullable(values!);

        public bool TryGetValues<T>(out List<T> values)
        {
            var type = typeof(T);
            
            if (!TryGetValue(type, out var objectList))
            {
                values = default!;
                return false;
            }

            values = (List<T>)objectList;
            return true;
        }
    }
}