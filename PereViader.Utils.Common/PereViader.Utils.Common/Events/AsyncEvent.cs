using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Attributes;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.Events
{
    [Experimental]
    public sealed class AsyncEvent<T> : IAsyncEvent<T>
    {
        public event Action<T> OnEvent;
        
        private readonly List<Func<CancellationToken, Task>> _instantListeners = new List<Func<CancellationToken, Task>>();
        private readonly List<Func<CancellationToken, Task>> _sequencedListeners = new List<Func<CancellationToken, Task>>();

        private readonly TaskRunner _taskRunner = new TaskRunner();

        public int ListenerCount => InstantListenerCount + SequencedListenerCount;
        public int InstantListenerCount => _instantListeners.Count;
        public int SequencedListenerCount => _sequencedListeners.Count;

        public void AddInstantListener(Func<CancellationToken, Task> func)
        {
            _instantListeners.Add(func);
        }

        public void RemoveInstantListener(Func<CancellationToken, Task> func)
        {
            _instantListeners.Remove(func);
        }

        public void AddSequencedListener(Func<CancellationToken, Task> func)
        {
            _sequencedListeners.Add(func);
        }

        public void RemoveSequencedListener(Func<CancellationToken, Task> func)
        {
            _sequencedListeners.Remove(func);
        }

        public Task Raise(T value, CancellationToken ct)
        {
            OnEvent?.Invoke(value);
            
            return Task.WhenAll(
                _taskRunner.RunInstantly(_instantListeners, ct),
                _taskRunner.RunSequencedAndTrack(_sequencedListeners, ct)
            );
        }

        public void ClearAllListeners()
        {
            _instantListeners.Clear();
            _sequencedListeners.Clear();
        }
    }
}