using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.DynamicDispatch
{
    public sealed class DynamicDispatchStore<T>
    {
        readonly Dictionary<Type, T> _values = new Dictionary<Type, T>();

        public bool TryGet(
            Type type,
            bool checkAssignableTypes,
            out T value)
        {
            if (_values.TryGetValue(type, out value))
            {
                return true;
            }

            if (!checkAssignableTypes || !TryGetAssignableKeyType(type, out var assignableKeyType))
            {
                return false;
            }

            return _values.TryGetValue(assignableKeyType, out value);
        }

        bool TryGetAssignableKeyType(Type type, out Type assignableKeyType)
        {
            foreach (var value in _values)
            {
                if (value.Key.IsAssignableFrom(type))
                {
                    assignableKeyType = value.Key;
                    return true;
                }
            }

            assignableKeyType = default;
            return false;
        }

        public T this[Type key]
        {
            get => _values[key];
            set => _values[key] = value;
        }
    }
}
