#nullable enable
using System;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

using System.Collections.Generic;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.TaskRunners
{
    public sealed class TaskRunner : IDisposable
    {
        private interface ITaskRunnerQueueElement : IDisposable
        {
            Task Run();
        }

        private sealed class FuncTaskRunnerQueueElement : ITaskRunnerQueueElement
        {
            public Func<CancellationToken, Task> Func { get; }
            
            public TaskCompletionSource<object?> TaskCompletionSource { get; }
            public CancellationTokenSource CancellationTokenSource { get; }

            public FuncTaskRunnerQueueElement(Func<CancellationToken, Task> func, TaskCompletionSource<object?> taskCompletionSource, CancellationTokenSource cancellationTokenSource)
            {
                Func = func;
                TaskCompletionSource = taskCompletionSource;
                CancellationTokenSource = cancellationTokenSource;
                TaskCompletionSource.LinkCancellationToken(CancellationTokenSource.Token);
            }
            
            public async Task Run()
            {
                if (CancellationTokenSource.Token.IsCancellationRequested)
                {
                    TaskCompletionSource.TrySetCanceled();
                    return;
                }

                try
                {
                    await Func(CancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    TaskCompletionSource.TrySetCanceled();
                }
                catch (Exception ex)
                {
                    TaskCompletionSource.TrySetException(ex);
                }
                finally
                {
                    TaskCompletionSource.TrySetResult(null);
                }
            }

            public void Dispose()
            {
                CancellationTokenSource.Dispose();
            }
        }
        
        private sealed class EnumerableFuncTaskRunnerQueueElement : ITaskRunnerQueueElement
        {
            public IEnumerable<Func<CancellationToken, Task>> Funcs { get; }
            
            public TaskCompletionSource<object?> TaskCompletionSource { get; }
            public CancellationTokenSource CancellationTokenSource { get; }
            private bool StopOnFirstException { get; }

            public EnumerableFuncTaskRunnerQueueElement(IEnumerable<Func<CancellationToken, Task>> funcs, TaskCompletionSource<object?> taskCompletionSource, CancellationTokenSource cancellationTokenSource, bool stopOnFirstException)
            {
                Funcs = funcs;
                TaskCompletionSource = taskCompletionSource;
                CancellationTokenSource = cancellationTokenSource;
                StopOnFirstException = stopOnFirstException;
                TaskCompletionSource.LinkCancellationToken(CancellationTokenSource.Token);
            }
            
            public async Task Run()
            {
                if (CancellationTokenSource.Token.IsCancellationRequested)
                {
                    TaskCompletionSource.TrySetCanceled();
                    return;
                }

                if(StopOnFirstException)
                {
                    try
                    {
                        foreach (var func in Funcs)
                        {
                            await func(CancellationTokenSource.Token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        TaskCompletionSource.TrySetCanceled();
                    }
                    catch (Exception ex)
                    {
                        TaskCompletionSource.TrySetException(ex);
                    }
                }
                else
                {
                    bool isCanceled = false;
                    List<Exception>? exceptions = null;
                    foreach (var func in Funcs)
                    {
                        try
                        {
                            await func(CancellationTokenSource.Token);
                        }
                        catch (OperationCanceledException ex)
                        {
                            isCanceled = true;
                            exceptions?.Add(ex);
                            break;
                        }
                        catch (Exception ex)
                        {
                            exceptions ??= new List<Exception>();
                            exceptions.Add(ex);
                        }
                    }

                    if (exceptions != null)
                    {
                        TaskCompletionSource.TrySetException(exceptions);
                    }
                    else if (isCanceled)
                    {
                        TaskCompletionSource.TrySetCanceled();
                    }
                }
                
                TaskCompletionSource.TrySetResult(null);
            }

            public void Dispose()
            {
                CancellationTokenSource.Dispose();
            }
        }

        private sealed class FuncWithStateTaskRunnerQueueElement<T> : ITaskRunnerQueueElement
        {
            public Func<T, CancellationToken, Task> Func { get; }
            public T State { get; }
            public TaskCompletionSource<object?> TaskCompletionSource { get; }
            public CancellationTokenSource CancellationTokenSource { get; }

            public FuncWithStateTaskRunnerQueueElement(Func<T, CancellationToken, Task> func, T state, TaskCompletionSource<object?> taskCompletionSource, CancellationTokenSource cancellationTokenSource)
            {
                Func = func;
                State = state;
                TaskCompletionSource = taskCompletionSource;
                CancellationTokenSource = cancellationTokenSource;
            }
            
            public async Task Run()
            {
                if (CancellationTokenSource.Token.IsCancellationRequested)
                {
                    TaskCompletionSource.TrySetCanceled();
                    return;
                }

                try
                {
                    await Func(State, CancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    TaskCompletionSource.TrySetCanceled();
                }
                catch (Exception ex)
                {
                    TaskCompletionSource.TrySetException(ex);
                }
                finally
                {
                    TaskCompletionSource.TrySetResult(null);
                }
            }

            public void Dispose()
            {
                CancellationTokenSource.Dispose();
            }
        }
        
        private sealed class EnumerableFuncWithStateTaskRunnerQueueElement<T> : ITaskRunnerQueueElement
        {
            public IEnumerable<Func<T, CancellationToken, Task>> Funcs { get; }
            public TaskCompletionSource<object?> TaskCompletionSource { get; }
            public CancellationTokenSource CancellationTokenSource { get; }
            public T State { get; }
            private bool StopOnFirstException { get; }

            public EnumerableFuncWithStateTaskRunnerQueueElement(IEnumerable<Func<T, CancellationToken, Task>> funcs, TaskCompletionSource<object?> taskCompletionSource, CancellationTokenSource cancellationTokenSource, bool stopOnFirstException, T state)
            {
                Funcs = funcs;
                TaskCompletionSource = taskCompletionSource;
                CancellationTokenSource = cancellationTokenSource;
                StopOnFirstException = stopOnFirstException;
                State = state;
                TaskCompletionSource.LinkCancellationToken(CancellationTokenSource.Token);
            }
            
            public async Task Run()
            {
                if (CancellationTokenSource.Token.IsCancellationRequested)
                {
                    TaskCompletionSource.TrySetCanceled();
                    return;
                }

                if(StopOnFirstException)
                {
                    try
                    {
                        foreach (var func in Funcs)
                        {
                            await func(State, CancellationTokenSource.Token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        TaskCompletionSource.TrySetCanceled();
                    }
                    catch (Exception ex)
                    {
                        TaskCompletionSource.TrySetException(ex);
                    }
                }
                else
                {
                    bool isCanceled = false;
                    List<Exception>? exceptions = null;
                    foreach (var func in Funcs)
                    {
                        try
                        {
                            await func(State, CancellationTokenSource.Token);
                        }
                        catch (OperationCanceledException ex)
                        {
                            isCanceled = true;
                            exceptions?.Add(ex);
                            break;
                        }
                        catch (Exception ex)
                        {
                            exceptions ??= new List<Exception>();
                            exceptions.Add(ex);
                        }
                    }

                    if (exceptions != null)
                    {
                        TaskCompletionSource.TrySetException(exceptions);
                    }
                    else if (isCanceled)
                    {
                        TaskCompletionSource.TrySetCanceled();
                    }
                }
                
                TaskCompletionSource.TrySetResult(null);
            }

            public void Dispose()
            {
                CancellationTokenSource.Dispose();
            }
        }

        private readonly Queue<ITaskRunnerQueueElement> _taskSequenceQueue = new Queue<ITaskRunnerQueueElement>();

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

        public async Task RunSequenced(Func<CancellationToken, Task> func, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }
            
            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);
            var taskCompletionSource = cancellationTokenSource.Token.CreateLinkedTaskCompletionSource<object?>();
            _taskSequenceQueue.Enqueue(new FuncTaskRunnerQueueElement(func, taskCompletionSource, cancellationTokenSource));

            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }

            await taskCompletionSource.Task;
        }
        
        public async Task RunSequenced<TArg>(Func<TArg, CancellationToken, Task> func, TArg arg, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }
            
            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);
            var taskCompletionSource = cancellationTokenSource.Token.CreateLinkedTaskCompletionSource<object?>();
            _taskSequenceQueue.Enqueue(new FuncWithStateTaskRunnerQueueElement<TArg>(func, arg, taskCompletionSource, cancellationTokenSource));

            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }

            await taskCompletionSource.Task;
        }
        
        public async Task RunSequenced(IEnumerable<Func<CancellationToken, Task>> funcs, bool stopOnFirstException = false, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);
            var taskCompletionSource = cancellationTokenSource.Token.CreateLinkedTaskCompletionSource<object?>();
            _taskSequenceQueue.Enqueue(new EnumerableFuncTaskRunnerQueueElement(funcs, taskCompletionSource, cancellationTokenSource, stopOnFirstException));
            
            if (!_isRunning)
            {
                ProcessSequenceQueue();
            }

            await taskCompletionSource.Task;
        }

        public async Task RunSequenced<TArg>(IEnumerable<Func<TArg, CancellationToken, Task>> funcs, TArg arg, 
            bool stopOnFirstException = false, CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("TaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token);
            var taskCompletionSource = cancellationTokenSource.Token.CreateLinkedTaskCompletionSource<object?>();
            _taskSequenceQueue.Enqueue(new EnumerableFuncWithStateTaskRunnerQueueElement<TArg>(funcs, taskCompletionSource, cancellationTokenSource, stopOnFirstException, arg));
            
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
                    using var request = _taskSequenceQueue.Dequeue();
                    await request.Run();
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

            var count = _taskSequenceQueue.Count;
            var array = ArrayPool<ITaskRunnerQueueElement>.Shared.Rent(count);
            _taskSequenceQueue.CopyTo(array, 0);
            
            _taskSequenceQueue.Clear();
            
            _cancellationTokenSource.Cancel(); //This will also cancel all linked cancellation tokens created within this class
            _cancellationTokenSource.Dispose();

            for (int i = 0; i < count; i++)
            {
                array[i].Dispose();
            }
            
            ArrayPool<ITaskRunnerQueueElement>.Shared.Return(array, true);
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