using System;

namespace PereViader.Utils.Common.Disposables
{
    public interface IAsyncDisposable<out T> : IAsyncDisposable
    {
        T Value { get; }
    }
}