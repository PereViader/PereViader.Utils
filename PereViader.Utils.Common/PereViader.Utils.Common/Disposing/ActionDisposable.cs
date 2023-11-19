using System;

namespace PereViader.Utils.Common.Runtime.Disposing
{
    public sealed class ActionDisposable<T> : IDisposable<T>
    {
        private readonly Action _action;
        private bool _disposed;

        public ActionDisposable(T value, Action action)
        {
            this._action = action;
            this.Value = value;
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