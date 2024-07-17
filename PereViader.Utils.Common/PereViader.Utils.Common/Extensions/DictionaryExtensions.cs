using System.Collections.Generic;

namespace PereViader.Utils.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Removes a range of keys from the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary from which to remove the keys.</param>
        /// <param name="keys">The keys to remove from the dictionary.</param>
        public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                dictionary.Remove(key);
            }
        }
        
        /// <summary>
        /// Removes a range of keys from the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary from which to remove the keys.</param>
        /// <param name="keys">The keys to remove from the dictionary.</param>
        public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IReadOnlyList<TKey> keys)
        {
            var keysCount = keys.Count;
            for (int i = 0; i < keysCount; i++)
            {
                dictionary.Remove(keys[i]);
            }
        }
    }
}