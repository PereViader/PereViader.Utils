using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Extensions
{
    public static class TaskCompletionSourceExtensions
    {
        public static CancellationTokenRegistration LinkCancellationToken<T>(
            this TaskCompletionSource<T> taskCompletionSource,
            CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
            {
                return default;
            }

            return cancellationToken.Register(state =>
            {
                ((TaskCompletionSource<T>)state).TrySetCanceled();
            }, taskCompletionSource);        
        }
    }
}