using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Disposables
{
    public struct ValueAsyncDisposable<T> : IAsyncDisposable<T>
    {
        private readonly Func<T, ValueTask> _func;
        private bool _disposed;

        public ValueAsyncDisposable(T value, Func<T, ValueTask> func)
        {
            _func = func;
            _disposed = false;
            Value = value;
        }

        public T Value { get; }
        
        public ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return default;
            }

            _disposed = true;
            return _func.Invoke(Value);
        }
        
        public AsyncDisposable<T> AsAsyncDisposable() => new(Value, _func);
    }
}