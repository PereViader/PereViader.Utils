using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Disposables
{
    public sealed class AsyncDisposable : IAsyncDisposable
    {
        private readonly Func<ValueTask> _func;
        private bool _disposed;

        public AsyncDisposable(Func<ValueTask> func)
        {
            _func = func;
        }
        
        public ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return default;
            }

            _disposed = true;
            return _func.Invoke();
        }
    }
    
    public sealed class AsyncDisposable<T> : IAsyncDisposable<T>
    {
        private readonly Func<T, ValueTask> _func;
        private bool _disposed;

        public AsyncDisposable(T value, Func<T, ValueTask> func)
        {
            _func = func;
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
    }
}