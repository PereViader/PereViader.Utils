using System;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace PereViader.Utils.Common.TaskRunners
{
    public sealed class SequencedTaskRunner : IDisposable
    {
        private readonly Queue<Func<CancellationToken, Task>> _taskQueue = new Queue<Func<CancellationToken, Task>>();
        private CancellationTokenSource? _runCancellationTokenSource;
        private bool _isDisposed;

        public void RunWithoutCompletion(Func<CancellationToken, Task> func)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
            }

            _taskQueue.Enqueue(func);
            if (_runCancellationTokenSource == null)
            {
                ProcessQueue();
            }
        }
        
        public Task RunWithCompletion(Func<CancellationToken, Task> func)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("SequencedTaskRunner", "Cannot run task on a disposed SequencedTaskRunner.");
            }

            TaskCompletionSource<object?> taskCompletionSource = new TaskCompletionSource<object?>();

            RunWithoutCompletion(async ct =>
            {
                try
                {
                    await func(ct); 
                    taskCompletionSource.SetResult(default);
                }
                catch (OperationCanceledException)
                {
                    taskCompletionSource.TrySetCanceled();
                }
                catch (Exception ex)
                {
                    taskCompletionSource.SetException(ex);
                    throw;
                }
            });

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
                    var taskToRun = _taskQueue.Dequeue();
                    await taskToRun(token);
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