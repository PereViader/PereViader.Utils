#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace PereViader.Utils.Common.TaskRunners
{
    public sealed class TaskRunner : IDisposable
    {
        private readonly Queue<(TaskCompletionSource<object?>? completionTaskCompletionSource, Func<CancellationToken, Task> func)> _taskSequenceQueue = 
            new Queue<(TaskCompletionSource<object?>? completionTaskCompletionSource, Func<CancellationToken, Task> func)>();

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

        public Task RunInstantly<TArg>(Func<CancellationToken, TArg, Task> func, TArg arg)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            return func(_cancellationTokenSource.Token, arg);
        }
        
        public void RunSequencedAndForget(Func<CancellationToken, Task> func)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
            }

            _taskSequenceQueue.Enqueue((null, func));
            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }
        }

        public Task RunSequencedAndTrack(Func<CancellationToken, Task> func)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
            }

            var taskCompletionSource = new TaskCompletionSource<object?>();

            _taskSequenceQueue.Enqueue((taskCompletionSource, func));
            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }

            return taskCompletionSource.Task;
        }

        private async void ProcessSequenceQueue()
        {
            _isRunning = true;
            var token = _cancellationTokenSource.Token;
            try
            {
                while (_taskSequenceQueue.Count > 0)
                {
                    var (completion, taskToRun) = _taskSequenceQueue.Peek();

                    try
                    {
                        await taskToRun(token);
                    }
                    catch (TaskCanceledException)
                    {
                        completion?.SetCanceled();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        completion?.SetException(ex);
                        throw;
                    }

                    completion?.TrySetResult(null);
                    _taskSequenceQueue.Dequeue();
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

            foreach (var elements in _taskSequenceQueue)
            {
                elements.completionTaskCompletionSource?.SetCanceled();
            }

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