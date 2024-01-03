using System;

namespace PereViader.Utils.Common.Events
{
    public readonly struct EventRef<T>
        where T : Delegate
    {
        public object ObjectReference { get; }
        private readonly Action<object, T> subscribeAction;
        private readonly Action<object, T> unsubscribeAction;
        
        public EventRef(object value, Action<object, T> subscribeAction, Action<object, T> unsubscribeAction)
        {
            ObjectReference = value;
            this.subscribeAction = subscribeAction;
            this.unsubscribeAction = unsubscribeAction;
        }

        public void Subscribe(T action)
        {
            subscribeAction.Invoke(ObjectReference, action);
        }
        
        public void Unsubscribe(T action)
        {
            unsubscribeAction.Invoke(ObjectReference, action);
        }
    }
}