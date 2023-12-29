using System;

namespace PereViader.Utils.Common.DynamicDispatch
{
    public sealed class FuncDynamicDispatch<TObj, TResult>
    {
        private readonly DynamicDispatchStore<Func<TObj, TResult>> _store = new DynamicDispatchStore<Func<TObj, TResult>>();

        public void Register<TConcrete>(Func<TConcrete, TResult> func)
            where TConcrete : TObj
        {
            TResult InverseFunc(TObj obj) => func.Invoke((TConcrete)obj!);

            _store[typeof(TConcrete)] = InverseFunc;
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
            if (!_store.TryGet(obj!.GetType(), checkAssignableTypes, out var func))
            {
                result = default!;
                return false;
            }

            result = func.Invoke(obj);
            return true;
        }
    }

    public sealed class FuncDynamicDispatch<TObj, TArg1, TResult>
    {
        private readonly DynamicDispatchStore<Func<TObj, TArg1, TResult>> _store = new DynamicDispatchStore<Func<TObj, TArg1, TResult>>();

        public void Register<TConcrete>(Func<TConcrete, TArg1, TResult> func)
            where TConcrete : TObj
        {
            TResult InverseFunc(
                TObj obj,
                TArg1 arg1
                ) => func.Invoke((TConcrete)obj!, arg1);

            _store[typeof(TConcrete)] = InverseFunc;
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
            if (!_store.TryGet(obj!.GetType(), checkAssignableTypes, out var func))
            {
                result = default!;
                return false;
            }

            result = func.Invoke(obj, arg1);
            return true;
        }
    }
}
