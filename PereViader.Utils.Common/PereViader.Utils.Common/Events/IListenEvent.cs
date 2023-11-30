using System;

namespace PereViader.Utils.Common.Events
{
    public interface IListenEvent<out T>
    {
        event Action<T> OnEvent;

        void AddListener(Action<T> action);
        void RemoveListener(Action<T> action);
    }
}