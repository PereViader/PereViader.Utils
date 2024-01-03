using System;
using PereViader.Utils.Common.Collections;

namespace PereViader.Utils.Common.DynamicDispatch
{
    public sealed class ActionDynamicDispatch<TObj>
    {
        private readonly TypeDictionary<(Action<TObj,object> inverseAction, object originalAction)> _store = new();

        public void Register<TConcrete>(Action<TConcrete> action)
            where TConcrete : TObj
        {
            Action<TObj,object> action1 = (obj, actionObject) => ((Action<TConcrete>)actionObject).Invoke((TConcrete)obj!);
            
            _store[typeof(TConcrete)] = (action1, action);
        }

        public void Execute(TObj param)
        {
            if (!TryExecute(param))
            {
                throw new InvalidOperationException();
            }
        }

        public bool TryExecute(
            TObj obj, 
            bool checkAssignableTypes = true)
        {
            if (!_store.TryGetValue(obj!.GetType(), checkAssignableTypes, out var context))
            {
                return false;
            }

            context.inverseAction.Invoke(obj, context.originalAction);
            return true;
        }
    }

    public sealed class ActionDynamicDispatch<TObj, TArg1>
    {
        private readonly TypeDictionary<(Action<TObj, TArg1, object> inverseAction, object originalAction)> _store = new();

        public void Register<TConcrete>(Action<TConcrete, TArg1> action)
            where TConcrete : TObj
        {
            Action<TObj, TArg1, object> inverseAction = (
                obj,
                arg1,
                originalAction
                ) => ((Action<TConcrete, TArg1>)originalAction).Invoke((TConcrete)obj!, arg1);

            _store[typeof(TConcrete)] = (inverseAction, action);
        }

        public void Execute(TObj obj, TArg1 arg1)
        {
            if (!TryExecute(obj, arg1))
            {
                throw new InvalidOperationException();
            }
        }

        public bool TryExecute(
            TObj obj,
            TArg1 arg1, 
            bool checkAssignableTypes = true)
        {
            if (!_store.TryGetValue(obj!.GetType(), checkAssignableTypes, out var context))
            {
                return false;
            }

            context.inverseAction.Invoke(obj, arg1, context.originalAction);
            return true;
        }
    }
}
