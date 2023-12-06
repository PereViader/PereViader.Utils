using System;

namespace PereViader.Utils.Common.Disposables
{
    public interface IDisposable<out T> : IDisposable
    {
        T Value { get; }
    }
}