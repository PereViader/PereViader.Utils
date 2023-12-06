#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace PereViader.Utils.Common.TaskRunners
{
    public sealed class SequencedTaskRunner : IDisposable
    {
        private readonly Queue<(TaskCompletionSource<object?>? completionTaskCompletionSource, Func<CancellationToken, Task> func)> _taskQueue = 
            new Queue<(TaskCompletionSource<object?>? completionTaskCompletionSource, Func<CancellationToken, Task> func)>();
        
        private CancellationTokenSource? _runCancellationTokenSource;
        private bool _isDisposed;

        public void RunAndForget(Func<CancellationToken, Task> func)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
            }

            _taskQueue.Enqueue((null, func));
            if (_runCancellationTokenSource == null)
            {
                ProcessQueue();
            }
        }
        
        public Task RunAndTrack(Func<CancellationToken, Task> func)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
            }

            var taskCompletionSource = new TaskCompletionSource<object?>();

            _taskQueue.Enqueue((taskCompletionSource, func));
            if (_runCancellationTokenSource == null)
            {
                ProcessQueue();
            }

            return taskCompletionSource.Task;
        }

        private async void ProcessQueue()
        {
            _runCancellationTokenSource = new CancellationTokenSource();
            var token = _runCancellationTokenSource.Token;
            try
            {
                while (_taskQueue.Count > 0)
                {
                    var (completion, taskToRun) = _taskQueue.Peek();
                    
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
                    _taskQueue.Dequeue();
                }
            }
            finally
            {
                //When cancellation is requested, cleanup is done on the cancellation method
                if (!token.IsCancellationRequested)
                {
                    _runCancellationTokenSource.Dispose();
                    _runCancellationTokenSource = null;
                }
            }
        }

        public void CancelRunning()
        {
            if (_isDisposed || _runCancellationTokenSource == null)
            {
                return;
            }

            foreach (var elements in _taskQueue)        
            {
                elements.completionTaskCompletionSource?.SetCanceled();
            }
            
            _taskQueue.Clear();
            _runCancellationTokenSource.Cancel();
            _runCancellationTokenSource.Dispose();
            _runCancellationTokenSource = null;
        }

        public void Dispose()
        {
            CancelRunning();
            _isDisposed = true;
        }
    }
}