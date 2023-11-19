using System;

namespace PereViader.Utils.Common.Runtime.Disposing
{
    public sealed class ActionDisposable<T> : IDisposable<T>
    {
        private readonly Action action;
        private bool disposed;

        public ActionDisposable(T value, Action action)
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
            action.Invoke();
        }

        public T Value { get; }
    }
}