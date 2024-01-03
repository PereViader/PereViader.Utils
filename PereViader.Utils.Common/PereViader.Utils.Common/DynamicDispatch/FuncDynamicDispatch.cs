using System;

namespace PereViader.Utils.Common.DynamicDispatch
{
    public sealed class FuncDynamicDispatch<TObj, TResult>
    {
        private readonly DynamicDispatchStore<(Func<TObj, object, TResult> inverseFunc, object originalFunc)> _store = new();

        public void Register<TConcrete>(Func<TConcrete, TResult> func)
            where TConcrete : TObj
        {
            Func<TObj, object, TResult> inverseFunc = (obj, originalFunc) => ((Func<TConcrete, TResult>)originalFunc).Invoke((TConcrete)obj!);

            _store[typeof(TConcrete)] = (inverseFunc, func);
        }

        public TResult Execute(TObj param)
        {
            if (!TryExecute(param, out var result))
            {
                throw new InvalidOperationException();
            }

            return result;
        }

        public bool TryExecute(
            TObj obj,
            out TResult result,
            bool checkAssignableTypes = true)
        {
            if (!_store.TryGet(obj!.GetType(), checkAssignableTypes, out var context))
            {
                result = default!;
                return false;
            }

            result = context.inverseFunc.Invoke(obj, context.originalFunc);
            return true;
        }
    }

    public sealed class FuncDynamicDispatch<TObj, TArg1, TResult>
    {
        private readonly DynamicDispatchStore<(Func<TObj, TArg1, object, TResult> inverseFunc, object originalFunc)> _store = new();

        public void Register<TConcrete>(Func<TConcrete, TArg1, TResult> func)
            where TConcrete : TObj
        {
            Func<TObj, TArg1, object, TResult> inverseFunc = (obj, arg1, originalFunc) => ((Func<TConcrete, TArg1, TResult>)originalFunc).Invoke((TConcrete)obj!, arg1);

            _store[typeof(TConcrete)] = (inverseFunc, func);
        }

        public TResult Execute(
            TObj param, 
            TArg1 arg1,
            bool checkAssignableTypes = true)
        {
            if (!TryExecute(param, arg1, out var result, checkAssignableTypes))
            {
                throw new InvalidOperationException();
            }

            return result;
        }

        public bool TryExecute(
            TObj obj,
            TArg1 arg1,
            out TResult result,
            bool checkAssignableTypes = true)
        {
            if (!_store.TryGet(obj!.GetType(), checkAssignableTypes, out var context))
            {
                result = default!;
                return false;
            }

            result = context.inverseFunc.Invoke(obj, arg1, context.originalFunc);
            return true;
        }
    }
}
