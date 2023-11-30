namespace PereViader.Utils.Common.Events
{
    public interface IEvent<T> : IListenEvent<T>
    {
        int ListenerCount { get; }
        
        void Raise(T value);
        void ClearAllListeners();
    }
}