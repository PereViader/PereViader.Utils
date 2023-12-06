﻿using System;

namespace PereViader.Utils.Common.Disposables
{
    public sealed class ActionDisposable<T> : IDisposable<T>
    {
        private readonly Action _action;
        private bool _disposed;

        public ActionDisposable(T value, Action action)
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
            _action.Invoke();
        }

        public T Value { get; }
    }
}