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

        private readonly List<Action<T>> eventActions = new List<Action<T>>();

        public int ListenerCount => eventActions.Count;
        
        public void AddListener(Action<T> action)
        {
            eventActions.Add(action);
        }

        public void RemoveListener(Action<T> action)
        {
            eventActions.Remove(action);
        }

        public void ClearAllListeners()
        {
            eventActions.Clear();
        }

        public void Raise(T value)
        {
            var count = ListenerCount;
            var cache = ArrayPool<Action<T>>.Shared.Rent(count);
            eventActions.CopyTo(cache);
            for (int i = 0; i < count; i++)
            {
                var action = cache[i];
                action.Invoke(value);
            }
            ArrayPool<Action<T>>.Shared.Return(cache, true);
        }
    }
}