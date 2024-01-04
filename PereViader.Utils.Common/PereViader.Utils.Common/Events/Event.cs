using System;
using System.Buffers;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Events
{
    public sealed class Event<T> : IEvent<T>
    {
        public event Action<T> OnEvent
        {
            add => AddListener(value);
            remove => RemoveListener(value);
        }

        private readonly List<Action<T>> _eventActions = new();

        public int ListenerCount => _eventActions.Count;
        
        public void AddListener(Action<T> action)
        {
            _eventActions.Add(action);
        }

        public void RemoveListener(Action<T> action)
        {
            _eventActions.Remove(action);
        }

        public void ClearAllListeners()
        {
            _eventActions.Clear();
        }

        public void Raise(T value)
        {
            var count = ListenerCount;
            var cache = ArrayPool<Action<T>>.Shared.Rent(count);
            _eventActions.CopyTo(cache);
            for (int i = 0; i < count; i++)
            {
                var action = cache[i];
                action.Invoke(value);
            }
            ArrayPool<Action<T>>.Shared.Return(cache, true);
        }
    }
}