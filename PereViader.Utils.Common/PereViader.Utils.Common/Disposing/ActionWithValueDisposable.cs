using System;

namespace PereViader.Utils.Common.Disposing
{
    public sealed class ActionWithValueDisposable<T> : IDisposable<T>
    {
        private readonly Action<T> _action;
        private bool _disposed;

        public ActionWithValueDisposable(T value, Action<T> action)
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
            _action.Invoke(Value);
        }

        public T Value { get; }
    }
}