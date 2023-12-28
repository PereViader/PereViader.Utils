using System.Collections;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Collections
{
    public class BiDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _keyToValue = new Dictionary<TKey, TValue>();
        private readonly Dictionary<TValue, TKey> _valueToKey = new Dictionary<TValue, TKey>();

        public IReadOnlyDictionary<TKey, TValue> KeyToValue => _keyToValue;
        public IReadOnlyDictionary<TValue, TKey> ValueToKey => _valueToKey;

        public TValue this[TKey key]
        {
            get => _keyToValue[key];
            set
            {
                if (_keyToValue.TryGetValue(key, out var oldVal))
                {
                    _valueToKey.Remove(oldVal);
                }
                _keyToValue[key] = value;
                _valueToKey[value] = key;
            }
        }

        public TKey this[TValue val]
        {
            get => _valueToKey[val];
            set
            {
                if (_valueToKey.TryGetValue(val, out var oldVal))
                {
                    _keyToValue.Remove(oldVal);
                }
                
                _valueToKey[val] = value;
                _keyToValue[value] = val;
            }
        }

        public ICollection<TKey> Keys => _keyToValue.Keys;

        public ICollection<TValue> Values => _valueToKey.Keys;

        public int Count => _keyToValue.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            _keyToValue[key] = value;
            _valueToKey[value] = key;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _keyToValue[item.Key] = item.Value;
            _valueToKey[item.Value] = item.Key;
        }

        public void Clear()
        {
            _keyToValue.Clear();
            _valueToKey.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => _keyToValue.ContainsKey(item.Key) && _valueToKey.ContainsKey(item.Value);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)_keyToValue).CopyTo(array, arrayIndex);

        public bool ContainsKey(TKey key) => _keyToValue.ContainsKey(key);

        public bool ContainsValue(TValue value) => _valueToKey.ContainsKey(value);
        
        public bool Remove(TKey key)
        {
            if (!_keyToValue.TryGetValue(key, out var value))
            {
                return false;
            }
            
            _valueToKey.Remove(value);
            _keyToValue.Remove(key);
            return true;
        }

        public bool Remove(TValue value)
        {
            if (!_valueToKey.TryGetValue(value, out var key))
            {
                return false;
            }
            
            _valueToKey.Remove(value);
            _keyToValue.Remove(key);
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public bool TryGetValue(TKey key, out TValue value) => _keyToValue.TryGetValue(key, out value);

        public bool TryGetValue(TValue value, out TKey key) => _valueToKey.TryGetValue(value, out key);
        
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyToValue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}