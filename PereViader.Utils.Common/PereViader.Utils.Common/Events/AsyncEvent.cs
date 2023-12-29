using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.Events
{
    public sealed class AsyncEvent<T> : IAsyncEvent<T>
    {
        public event Action<T>? OnEvent;
        
        private readonly List<Func<T, CancellationToken, Task>> _instantListeners = new List<Func<T, CancellationToken, Task>>();
        private readonly List<Func<T, CancellationToken, Task>> _sequencedListeners = new List<Func<T, CancellationToken, Task>>();

        private readonly TaskRunner _taskRunner = new TaskRunner();

        public int ListenerCount => InstantListenerCount + SequencedListenerCount;
        public int InstantListenerCount => _instantListeners.Count;
        public int SequencedListenerCount => _sequencedListeners.Count;

        public void AddInstantListener(Func<T, CancellationToken, Task> func)
        {
            _instantListeners.Add(func);
        }

        public void RemoveInstantListener(Func<T, CancellationToken, Task> func)
        {
            _instantListeners.Remove(func);
        }

        public void AddSequencedListener(Func<T, CancellationToken, Task> func)
        {
            _sequencedListeners.Add(func);
        }

        public void RemoveSequencedListener(Func<T, CancellationToken, Task> func)
        {
            _sequencedListeners.Remove(func);
        }

        public Task Raise(T value, CancellationToken ct)
        {
            OnEvent?.Invoke(value);
            
            return Task.WhenAll(
                _taskRunner.RunInstantly(_instantListeners, value, ct),
                _taskRunner.RunSequenced(_sequencedListeners, value, stopOnFirstException: false, cancellationToken: ct)
            );
        }

        public void ClearAllListeners()
        {
            _instantListeners.Clear();
            _sequencedListeners.Clear();
        }
    }
}