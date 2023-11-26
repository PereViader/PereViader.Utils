using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Attributes;

namespace PereViader.Utils.Common.Extensions
{
    public static class TaskExtensions
    {
        [Experimental]
        public static Task<T> CreateLinkedTask<T>(this Task<T> task, CancellationToken cancellationToken = default)
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
        
        [Experimental]
        public static Task CreateLinkedTask(this Task task, CancellationToken cancellationToken = default)
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
            return Task.WhenAny(task, cancellableTask)
                .Unwrap();
        }
        
        public static async Task WhenFirstCancelRest(
            IEnumerable<Func<CancellationToken, Task>> taskFuncs,
            CancellationToken ct)
        {
            using (var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                var token = cancellationTokenSource.Token;
                try
                {
                    await Task.WhenAny(
                        taskFuncs.Select(x => x.Invoke(token))
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
                        taskFuncs.Select(x => x.Invoke(token))
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