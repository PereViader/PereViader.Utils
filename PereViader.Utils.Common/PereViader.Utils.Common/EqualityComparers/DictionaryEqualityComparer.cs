using System.Collections.Generic;
using System.Linq;

namespace PereViader.Utils.Common.EqualityComparers
{
    public class DictionaryComparer<TKey, TValue> : IEqualityComparer<Dictionary<TKey, TValue>>
    {
        public static readonly DictionaryComparer<TKey, TValue> Default = new();

        private readonly IEqualityComparer<TValue> _valueComparer;

        public DictionaryComparer(IEqualityComparer<TValue>? valueComparer = null)
        {
            _valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
        }

        public bool Equals(Dictionary<TKey, TValue>? x, Dictionary<TKey, TValue>? y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            if (x.Count != y.Count) return false;

            foreach (var kvp in x)
            {
                if (!y.TryGetValue(kvp.Key, out var value)) return false;
                if (!_valueComparer.Equals(kvp.Value, value)) return false;
            }

            return true;
        }

        public int GetHashCode(Dictionary<TKey, TValue> obj)
        {
            int hash = 17;
            foreach (var kvp in obj.OrderBy(kvp => kvp.Key))
            {
                hash = hash * 31 + kvp.Key?.GetHashCode() ?? 0;
                hash = hash * 31 + _valueComparer.GetHashCode(kvp.Value);
            }

            return hash;
        }
    }
}