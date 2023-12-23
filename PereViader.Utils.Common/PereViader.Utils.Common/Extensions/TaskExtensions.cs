using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Extensions
{
    public static class TaskExtensions
    {
        public static Task<T> WaitAsync<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled || task.IsCompleted)
            {
                return task;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<T>(cancellationToken);
            }

            var cancellableTask = cancellationToken.CreateLinkedTask<T>();
            return Task.WhenAny(task, cancellableTask)
                .Unwrap();
        }
        
        public static Task WaitAsync(this Task task, CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled || task.IsCompleted)
            {
                return task;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            var cancellableTask = cancellationToken.CreateLinkedTask();
            return Task.WhenAny(task, cancellableTask);
        }
        
        public static async Task WhenAnyCancelRest(
            IEnumerable<Func<CancellationToken, Task>> taskFuncs,
            CancellationToken ct)
        {
            using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                var token = cancellationTokenSource.Token;
                try
                {
                    await Task.WhenAny(
                        taskFuncs.Select((x, t) => x.Invoke(t), token)
                    );
                }
                finally
                {
                    cancellationTokenSource.Cancel();
                }
            }
        }
        
        public static async Task<T> WhenAnyCancelRest<T>(
            IEnumerable<Func<CancellationToken, Task<T>>> taskFuncs,
            CancellationToken ct)
        {
            using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                var token = cancellationTokenSource.Token;
                try
                {
                    var task =  await Task.WhenAny(
                        taskFuncs.Select((x,t) => x.Invoke(t), token)
                    );
                    return task.Result;
                }
                finally
                {
                    cancellationTokenSource.Cancel();
                }
            }
        }
    }
}