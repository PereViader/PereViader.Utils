using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Events
{
    public interface IAsyncEvent<T> : IListenAsyncEvent<T>
    {
        int InstantListenerCount { get; }
        int SequencedListenerCount { get; }
        int ListenerCount { get; }
        
        Task Raise(T value, CancellationToken ct);
        void ClearAllListeners();
    }
}