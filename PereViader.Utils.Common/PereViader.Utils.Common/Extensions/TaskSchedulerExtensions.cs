using System;
using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Extensions
{
    public static class TaskSchedulerExtensions
    {
        /// <summary>
        /// Continue on the specified task scheduler, which becomes the current one
        /// Inspired by <see cref="https://github.com/dotnet/runtime/issues/20025"/>this GitHub issue</see>.
        /// Borrowed from https://gist.github.com/noseratio/5d2d5f2a0cbb71b7880ce731c3958e62
        /// </summary>
        /// <param name="taskScheduler">A task scheduler instance, e.g., <c>TaskScheduler.Default</c></param>
        /// <param name="alwaysSchedule">Always use the task scheduler to queue the continuations,
        /// even if it can be executed synchronously.
        /// </param>
        /// <example>
        /// <code>
        /// await TaskScheduler.Default.SwitchTo(alwaysSchedule: true);
        /// </code>
        /// </example> 
        /// <returns></returns>
        public static TaskSchedulerAwaitable SwitchTo(this TaskScheduler taskScheduler, bool alwaysSchedule = false)
        {
            return new TaskSchedulerAwaitable(taskScheduler, alwaysSchedule);
        }

        public struct TaskSchedulerAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion
        {
            private readonly TaskScheduler _scheduler;
            private bool _alwaysSchedule;

            public TaskSchedulerAwaiter(TaskScheduler scheduler, bool alwaysSchedule = false)
            {
                _scheduler = scheduler;
                _alwaysSchedule = alwaysSchedule;
            }

            private void Schedule(Action continuation)
            {
                Task.Factory.StartNew(
                    continuation,
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    _scheduler);
            }

            public bool IsCompleted =>
                // optimize if already on the default task scheduler
                // and on a thread pool thread without sync context
                !_alwaysSchedule &&
                _scheduler == TaskScheduler.Default &&
                TaskScheduler.Current == TaskScheduler.Default &&
                Thread.CurrentThread.IsThreadPoolThread &&
                SynchronizationContext.Current == null;

            public void GetResult() 
            { 
            }

            // a safe version that has to flow the execution context
            public void OnCompleted(Action continuation)
            {
                //It will never be called
                throw new InvalidOperationException(nameof(OnCompleted));
            }

            // an unsafe version that doesn't have to flow the execution context
            public void UnsafeOnCompleted(Action continuation)
            {
                // use ThreadPool.UnsafeQueueUserWorkItem to optimize for TaskScheduler.Default 
                if (_scheduler == TaskScheduler.Default)
                {
                    ThreadPool.UnsafeQueueUserWorkItem(
                        c => ((Action)c!).Invoke(), 
                        continuation);
                    return;
                }

                // use Task.Factory.StartNew for all non-default task schedulers 
                if (ExecutionContext.IsFlowSuppressed())
                {
                    Schedule(continuation);
                    return;
                }

                // suppress execution context flow
                ExecutionContext.SuppressFlow();
                try
                {
                    Schedule(continuation);
                }
                finally
                {
                    ExecutionContext.RestoreFlow();
                }
            }
        }

        public readonly struct TaskSchedulerAwaitable
        {
            private readonly TaskSchedulerAwaiter _awaiter;

            public TaskSchedulerAwaitable(TaskScheduler scheduler, bool alwaysSchedule = false)
            {
                _awaiter = new TaskSchedulerAwaiter(scheduler, alwaysSchedule);
            }

            public TaskSchedulerAwaiter GetAwaiter()
            {
                return _awaiter;
            }
        }
    }
}