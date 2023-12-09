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
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
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
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            var tasks = funcs.Select((x, ct) => x(ct), _cancellationTokenSource.Token);
            return Task.WhenAll(tasks);
        }
        
        public async Task RunInstantly(IEnumerable<Func<CancellationToken, Task>> funcs, CancellationToken cancellationToken)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);
            var tasks = funcs.Select((x, ct) => x(ct), linkedCts.Token);
            await Task.WhenAll(tasks);
        }

        public Task RunInstantly<TArg>(Func<CancellationToken, TArg, Task> func, TArg arg)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            return func(_cancellationTokenSource.Token, arg);
        }
        
        public async Task RunInstantly<TArg>(Func<CancellationToken, TArg, Task> func, TArg arg, CancellationToken cancellationToken)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }
            
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);
            await func(linkedCts.Token, arg);
        }
        
        public Task RunInstantly<TArg>(IEnumerable<Func<CancellationToken, TArg, Task>> funcs, TArg arg)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            var tasks = funcs.Select((x, pair) => x(pair.Token, pair.arg), (_cancellationTokenSource.Token, arg));
            return Task.WhenAll(tasks);
        }
        
        public async Task RunInstantly<TArg>(IEnumerable<Func<CancellationToken, TArg, Task>> funcs, TArg arg, CancellationToken cancellationToken)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token, cancellationToken);
            var tasks = funcs.Select((x, pair) => x(pair.Token, pair.arg), (linkedCts.Token, arg));
            await Task.WhenAll(tasks);
        }
        
        public void RunSequencedAndForget(Func<CancellationToken, Task> func, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
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
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
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
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
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
        
        public async Task RunSequencedAndTrack(IEnumerable<Func<CancellationToken, Task>> funcs, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
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
                        throw;
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