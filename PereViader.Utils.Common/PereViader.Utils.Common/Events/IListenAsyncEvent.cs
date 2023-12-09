using System;
using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Events
{
    public interface IListenAsyncEvent<out T>
    {
        event Action<T> OnEvent;

        void AddInstantListener(Func<T, CancellationToken, Task> func);
        void RemoveInstantListener(Func<T, CancellationToken, Task> func);
        void AddSequencedListener(Func<T, CancellationToken, Task> func);
        void RemoveSequencedListener(Func<T, CancellationToken, Task> func);
    }
}