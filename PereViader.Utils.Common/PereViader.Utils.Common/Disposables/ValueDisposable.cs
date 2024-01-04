using System;

namespace PereViader.Utils.Common.Disposables
{
    public struct ValueDisposable<T> : IDisposable<T>
    {
        private readonly Action<T> _action;
        private bool _disposed;
        
        public T Value { get; }

        public ValueDisposable(T value, Action<T> action)
        {
            _action = action;
            _disposed = false;
            Value = value;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _action.Invoke(Value);
        }
    }
}