using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Extensions
{
    public static class DelegateExtensions
    {
        public static class With
        {
            public static readonly Action DoNothingAction = () => { };
            public static readonly Func<bool> ReturnTrueFunc = () => true;
            public static readonly Func<bool> ReturnFalseFunc = () => true;
            public static readonly Func<Task> ReturnCompletedTaskFunc = () => Task.CompletedTask;
        }

        public static class With<TArg1>
        {
            public static readonly Action<TArg1> DoNothingAction = (arg1) => { };
            public static readonly Predicate<TArg1> ReturnTruePredicate = (arg1) => true;
            public static readonly Predicate<TArg1> ReturnFalsePredicate = (arg1) => false;
            public static readonly Func<TArg1, bool> ReturnTrueFunc = (arg1) => true;
            public static readonly Func<TArg1, bool> ReturnFalseFunc = (arg1) => false;
            public static readonly Func<TArg1, Task> ReturnCompletedTaskWithArg1Func = (arg1) => Task.FromResult(arg1);
            
            public static Predicate<TArg1> TrueAfterNCallsPredicate(int n)
            {
                return _ =>
                {
                    if (n <= 0)
                    {
                        return true;
                    }
                    
                    n--;
                    return false;
                };
            }
        }
        
        public static class With<TArg1, TArg2>
        {
            public static readonly Action<TArg1, TArg2> DoNothingAction = (arg1, arg2) => { };
            public static readonly Func<TArg1,TArg2, bool> ReturnTrueFunc = (arg1, arg2) => true;
            public static readonly Func<TArg1, TArg2, bool> ReturnFalseFunc = (arg1, arg2) => false;
            public static readonly Func<TArg1, TArg2, Task> ReturnCompletedTaskWithArg1Func = (arg1, arg2) => Task.FromResult(arg1);
            public static readonly Func<TArg1, TArg2, Task> ReturnCompletedTaskWithArg2Func = (arg1, arg2) => Task.FromResult(arg2);
        }
    }
}