using System;
using System.Collections;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class CoroutineExtensions
    {
        public static IEnumerator WaitFrame(Action action)
        {
            yield return null;
            action.Invoke();
        }
        
        public static IEnumerator WaitFrame<TArg>(Action<TArg> action, TArg arg)
        {
            yield return null;
            action.Invoke(arg);
        }
    }
}