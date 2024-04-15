using System;

namespace PereViader.Utils.Common.Disposables
{
    public sealed class Disposable : IDisposable
    {
        private readonly Action _action;
        private bool _disposed;
        
        public Disposable(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _action.Invoke();
        }
    }
    
    public sealed class Disposable<T> : IDisposable<T>
    {
        private readonly Action<T> _action;
        private bool _disposed;

        public T Value { get; }

        public Disposable(T value, Action<T> action)
        {
            _action = action;
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