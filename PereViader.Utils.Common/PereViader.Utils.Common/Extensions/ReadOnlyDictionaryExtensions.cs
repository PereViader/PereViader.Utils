using System.Collections.Generic;

namespace PereViader.Utils.Common.Extensions
{
    public static class ReadOnlyDictionaryExtensions
    {
        public static bool HasSameKeysAndValues<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict1, IReadOnlyDictionary<TKey, TValue> dict2, IEqualityComparer<TValue>? equalityComparer = null)
        {
            if (dict1 == dict2) return true;
            if (dict1.Count != dict2.Count) return false;

            var usedEqualityComparer = equalityComparer ?? EqualityComparer<TValue>.Default;

            foreach (var pair in dict1)
            {
                if (!dict2.TryGetValue(pair.Key, out var value) || !usedEqualityComparer.Equals(pair.Value, value))
                {
                    return false;
                }
            }

            return true;
        }
    
        public static bool HasSameKeys<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict1, IReadOnlyDictionary<TKey, TValue> dict2)
        {
            if (dict1 == dict2) return true;
            if (dict1.Count != dict2.Count) return false;

            foreach (var pair in dict1)
            {
                if (!dict2.ContainsKey(pair.Key)) return false;
            }

            return true;
        }
        
        public static IEnumerable<KeyValuePair<TKey, TValue>> GetDifferingEntries<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dict1, 
            IReadOnlyDictionary<TKey, TValue> dict2,
            IEqualityComparer<TValue>? equalityComparer = null)
        {
            var usedEqualityComparer = equalityComparer ?? EqualityComparer<TValue>.Default;
            
            foreach (var kvp in dict1)
            {
                if (!dict2.TryGetValue(kvp.Key, out TValue value2) || !usedEqualityComparer.Equals(kvp.Value, value2))
                {
                    yield return new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
                }
            }
        }
    }
}