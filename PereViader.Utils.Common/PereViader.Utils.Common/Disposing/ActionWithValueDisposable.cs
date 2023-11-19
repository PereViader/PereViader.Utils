using System;

namespace PereViader.Utils.Common.Runtime.Disposing
{
    public sealed class ActionWithValueDisposable<T> : IDisposable<T>
    {
        private readonly Action<T> action;
        private bool disposed;

        public ActionWithValueDisposable(T value, Action<T> action)
        {
            this.action = action;
            this.Value = value;
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            action.Invoke(Value);
        }

        public T Value { get; }
    }
}