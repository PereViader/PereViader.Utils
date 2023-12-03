using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Extensions
{
    public static class CancellationTokenExtensions
    {
        public static Task<T> CreateLinkedTask<T>(this CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<T>(cancellationToken);
            }
            
            TaskCompletionSource<T> taskCompletionSource = cancellationToken.CreateLinkedTaskCompletionSource<T>();
            return taskCompletionSource.Task;
        }
        
        public static Task CreateLinkedTask(this CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled(cancellationToken);
            }

            TaskCompletionSource<object> taskCompletionSource =
                cancellationToken.CreateLinkedTaskCompletionSource<object>();
            return taskCompletionSource.Task;
        }

        public static TaskCompletionSource<T> CreateLinkedTaskCompletionSource<T>(this CancellationToken cancellationToken)
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.LinkCancellationToken(cancellationToken);
            return taskCompletionSource;
        }
    }
}