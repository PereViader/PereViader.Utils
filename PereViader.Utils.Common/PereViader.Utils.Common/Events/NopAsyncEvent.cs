using System;
using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Events
{
    public sealed class NopAsyncEvent<T> : IAsyncEvent<T>
    {        
        public static readonly NopAsyncEvent<T> Instance = new NopAsyncEvent<T>();

        public event Action<T> OnEvent
        {
            add {}
            remove {}
        }

        public int InstantListenerCount => 0;
        public int SequencedListenerCount => 0;
        public int ListenerCount => 0;

        public void AddInstantListener(Func<T, CancellationToken, Task> func) { }
        public void RemoveInstantListener(Func<T, CancellationToken, Task> func) { }
        public void AddSequencedListener(Func<T, CancellationToken, Task> func) { }
        public void RemoveSequencedListener(Func<T, CancellationToken, Task> func) { }
        public Task Raise(T value, CancellationToken ct) => Task.CompletedTask;
        public void ClearAllListeners() { }
    }
}