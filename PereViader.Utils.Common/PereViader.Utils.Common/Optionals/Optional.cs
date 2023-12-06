using System;

namespace PereViader.Utils.Common.Optionals
{
    public readonly struct Optional<T>
    {
        private readonly T _value;
        public bool HasValue { get; }

        private Optional(T value, bool hasValue)
        {
            _value = value;
            HasValue = hasValue;
        }

        public static Optional<T> Some(T value)
        {
            if (value == null)
            {
                return None;
            }
            
            return new Optional<T>(value, true);
        }
        
        public static Optional<T> Maybe(T value, bool hasValue)
        {
            if (!hasValue)
            {
                return Optional<T>.None;
            }
            
            return Some(value);
        }

        public static Optional<T> None => default;

        public bool TryGetValue(out T value)
        {
            value = _value;
            return HasValue;
        }
        
        public T UnsafeGetValue()
        {
            if (!HasValue)
            {
                throw new InvalidOperationException($"No value present in {nameof(Optional<T>)}");
            }
            
            return _value;
        }
    }
}