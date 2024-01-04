﻿using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Disposables
{
    public interface IAsyncDisposable
    {
        Task DisposeAsync();
    }
    
    public interface IAsyncDisposable<out T> : IAsyncDisposable
    {
        T Value { get; }
    }

    public class AsyncDisposable<T> : IAsyncDisposable<T>
    {
        private readonly Func<T, Task> _func;
        private bool _disposed;

        public AsyncDisposable(T value, Func<T, Task> func)
        {
            _func = func;
            Value = value;
        }

        public T Value { get; }
        
        public Task DisposeAsync()
        {
            if (_disposed)
            {
                return Task.CompletedTask;
            }

            _disposed = true;
            return _func.Invoke(Value);
        }
    }
}