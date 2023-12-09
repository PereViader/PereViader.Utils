#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.TaskRunners
{
    public sealed class TaskRunner : IDisposable
    {
        private readonly Queue<(CancellationToken cancellationToken, Func<CancellationToken, Task> func)> _taskSequenceQueue = 
            new Queue<(CancellationToken cancellationToken, Func<CancellationToken, Task> func)>();

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private bool _isRunning;
        private bool _isDisposed;

        public Task RunInstantly(Func<CancellationToken, Task> func)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }
            
            return func(_cancellationTokenSource.Token);
        }
        
        public async Task RunInstantly(Func<CancellationToken, Task> func, CancellationToken cancellationToken)
        {
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);
            await func(linkedCts.Token);
        }
        
        public Task RunInstantly(IEnumerable<Func<CancellationToken, Task>> funcs)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            var tasks = funcs.Select((x, ct) => x(ct), _cancellationTokenSource.Token);
            return Task.WhenAll(tasks);
        }
        
        public async Task RunInstantly(IEnumerable<Func<CancellationToken, Task>> funcs, CancellationToken cancellationToken)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);
            var tasks = funcs.Select((x, ct) => x(ct), linkedCts.Token);
            await Task.WhenAll(tasks);
        }

        public Task RunInstantly<TArg>(Func<TArg, CancellationToken, Task> func, TArg arg)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            return func(arg, _cancellationTokenSource.Token);
        }
        
        public async Task RunInstantly<TArg>(Func<TArg, CancellationToken, Task> func, TArg arg, CancellationToken cancellationToken)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }
            
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);
            await func(arg, linkedCts.Token);
        }
        
        public Task RunInstantly<TArg>(IEnumerable<Func<TArg, CancellationToken, Task>> funcs, TArg arg)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            var tasks = funcs.Select((x, pair) => x(pair.arg, pair.Token), (_cancellationTokenSource.Token, arg));
            return Task.WhenAll(tasks);
        }
        
        public async Task RunInstantly<TArg>(IEnumerable<Func<TArg, CancellationToken, Task>> funcs, TArg arg, CancellationToken cancellationToken)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);
            var tasks = funcs.Select((x, pair) => x(pair.arg, pair.Token), (linkedCts.Token, arg));
            await Task.WhenAll(tasks);
        }
        
        public void RunSequencedAndForget(Func<CancellationToken, Task> func, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            _taskSequenceQueue.Enqueue((cancellationToken, func));
            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }
        }
        
        public void RunSequencedAndForget(IEnumerable<Func<CancellationToken, Task>> funcs, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            foreach (var func in funcs)
            {
                _taskSequenceQueue.Enqueue((cancellationToken, func));
            }
            
            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }
        }

        public async Task RunSequencedAndTrack(Func<CancellationToken, Task> func, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }
            
            _taskSequenceQueue.Enqueue((cancellationToken, func));
            
            using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                _cancellationTokenSource.Token, cancellationToken);
            var taskCompletionSource = cancellationTokenSource.Token.CreateLinkedTaskCompletionSource<object?>();
            
            _taskSequenceQueue.Enqueue((cancellationToken, ct =>
            {
                ct.ThrowIfCancellationRequested();
                taskCompletionSource.TrySetResult(null);
                return Task.CompletedTask;
            }));
            
            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }

            await taskCompletionSource.Task;
        }
        
        public Task RunSequencedAndTrack<TArg>(Func<TArg, CancellationToken, Task> func, TArg arg, CancellationToken cancellationToken = default)
        {
            // ReSharper disable once HeapView.CanAvoidClosure
            return RunSequencedAndTrack(ct => func(arg, ct), cancellationToken);
        }
        
        public async Task RunSequencedAndTrack(IEnumerable<Func<CancellationToken, Task>> funcs, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }
            
            foreach (var func in funcs)
            {
                _taskSequenceQueue.Enqueue((cancellationToken, func));
            }
            
            using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                _cancellationTokenSource.Token, cancellationToken);
            var taskCompletionSource = _cancellationTokenSource.Token.CreateLinkedTaskCompletionSource<object?>();

            _taskSequenceQueue.Enqueue((cancellationToken, ct =>
            {
                ct.ThrowIfCancellationRequested();
                taskCompletionSource.TrySetResult(null);
                return Task.CompletedTask;
            }));
            
            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }

            await taskCompletionSource.Task;
        }

        public Task RunSequencedAndTrack<TArg>(IEnumerable<Func<TArg, CancellationToken, Task>> funcs, TArg arg,
            CancellationToken cancellationToken = default)
        {
            var funcsWithoutArg = funcs.Select<Func<TArg, CancellationToken, Task>, TArg, Func<CancellationToken, Task>>((x, y) => ct => x(y, ct), arg);
            return RunSequencedAndTrack(funcsWithoutArg, cancellationToken);
        }

        private async void ProcessSequenceQueue()
        {
            _isRunning = true;
            var token = _cancellationTokenSource.Token;
            try
            {
                while (_taskSequenceQueue.Count > 0)
                {
                    var request = _taskSequenceQueue.Dequeue();

                    if (request.cancellationToken.IsCancellationRequested)
                    {
                        continue;
                    }
                    
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                        token, 
                        request.cancellationToken);
                    
                    try
                    {
                        await request.func(linkedCts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                    }
                }
            }
            finally
            {
                //When cancellation is requested, cleanup is done on the cancellation method
                if (!token.IsCancellationRequested)
                {
                    _isRunning = false;
                }
            }
        }

        public void CancelRunning()
        {
            if (_isDisposed)
            {
                return;
            }

            DoCancel();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void DoCancel()
        {
            _isRunning = false;

            _taskSequenceQueue.Clear();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            
            DoCancel();
            _isDisposed = true;
        }
    }
}