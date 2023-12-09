using System;
using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Events
{
    public interface IListenAsyncEvent<out T>
    {
        event Action<T> OnEvent;

        void AddInstantListener(Func<CancellationToken, Task> func);
        void RemoveInstantListener(Func<CancellationToken, Task> func);
        void AddSequencedListener(Func<CancellationToken, Task> func);
        void RemoveSequencedListener(Func<CancellationToken, Task> func);
    }
}