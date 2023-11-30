using System;

namespace PereViader.Utils.Common.Events
{
    public sealed class NopEvent<T> : IEvent<T>
    {
        public static readonly NopEvent<T> Instance = new NopEvent<T>();
        
        public event Action<T> OnEvent
        {
            add {}
            remove {}
        }
        
        public int ListenerCount => 0;

        private NopEvent()
        {
        }

        public void AddListener(Action<T> action)
        {
        }

        public void RemoveListener(Action<T> action)
        {
        }

        public void ClearAllListeners()
        {
        }

        public void Raise(T value)
        {
        }
    }
}