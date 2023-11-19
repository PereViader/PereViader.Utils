using System;

namespace PereViader.Utils.Common.DynamicDispatch
{
    public sealed class ActionDynamicDispatch<TObj>
    {
        readonly DynamicDispatchStore<Action<TObj>> _store = new DynamicDispatchStore<Action<TObj>>();

        public void Register<TConcrete>(Action<TConcrete> action)
            where TConcrete : TObj
        {
            void InverseAction(
                TObj obj) =>
                action.Invoke((TConcrete)obj);

            _store[typeof(TConcrete)] = InverseAction;
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
            if (!_store.TryGet(obj.GetType(), checkAssignableTypes, out var action))
            {
                return false;
            }

            action.Invoke(obj);
            return true;
        }
    }

    public sealed class ActionDynamicDispatch<TObj, TArg1>
    {
        readonly DynamicDispatchStore<Action<TObj, TArg1>> _store = new DynamicDispatchStore<Action<TObj, TArg1>>();

        public void Register<TConcrete>(Action<TConcrete, TArg1> action)
            where TConcrete : TObj
        {
            void InverseAction(
                TObj obj,
                TArg1 arg1
                ) => action.Invoke((TConcrete)obj, arg1);

            _store[typeof(TConcrete)] = InverseAction;
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
            if (!_store.TryGet(obj.GetType(), checkAssignableTypes, out var action))
            {
                return false;
            }

            action.Invoke(obj, arg1);
            return true;
        }
    }
}
