using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Extensions
{
    public static class CancellationTokenExtensions
    {
        public static Task<T> CreateLinkedTask<T>(this CancellationToken cancellationToken)
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.LinkCancellationToken(cancellationToken);
            return taskCompletionSource.Task;
        }
        
        public static Task CreateLinkedTask(this CancellationToken cancellationToken)
        {
            TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
            taskCompletionSource.LinkCancellationToken(cancellationToken);
            return taskCompletionSource.Task;
        }
    }
}