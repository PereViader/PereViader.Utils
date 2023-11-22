using System;

namespace PereViader.Utils.Common.Disposing
{
    public interface IDisposable<out T> : IDisposable
    {
        T Value { get; }
    }
}