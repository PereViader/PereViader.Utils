using System;

namespace PereViader.Utils.Common.Runtime.Disposing
{
    public interface IDisposable<out T> : IDisposable
    {
        T Value { get; }
    }
}