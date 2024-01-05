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

        public static Optional<T> Some(T value)=> value == null ? None : new Optional<T>(value, true);
        public static Optional<T> Maybe(T value, bool hasValue) => !hasValue ? None : Some(value);
        public static Optional<T> None => default;

        public bool TryGetValue(out T value)
        {
            value = _value;
            return HasValue;
        }
    }
}