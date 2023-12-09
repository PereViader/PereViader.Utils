using System;
using PereViader.Utils.Common.Generators;

namespace PereViader.Utils.Common.Events
{
    //[GenerateEventTaskWaits]
    public interface IListenEvent<out T>
    {
        event Action<T> OnEvent;

        void AddListener(Action<T> action);
        void RemoveListener(Action<T> action);
    }
}