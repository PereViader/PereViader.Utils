using System;

namespace PereViader.Utils.Common.Events
{
    public readonly struct EventRef<TObject, TDelegate>
        where TDelegate : Delegate
    {
        public TObject ObjectReference { get; }
        private readonly Action<TObject, TDelegate> subscribeAction;
        private readonly Action<TObject, TDelegate> unsubscribeAction;
        
        public EventRef(TObject value, Action<TObject, TDelegate> subscribeAction, Action<TObject, TDelegate> unsubscribeAction)
        {
            ObjectReference = value;
            this.subscribeAction = subscribeAction;
            this.unsubscribeAction = unsubscribeAction;
        }

        public void Subscribe(TDelegate action)
        {
            subscribeAction.Invoke(ObjectReference, action);
        }
        
        public void Unsubscribe(TDelegate action)
        {
            unsubscribeAction.Invoke(ObjectReference, action);
        }
    }
}