using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Extensions
{
    public static class DelegateExtensions
    {
        public static class With
        {
            public static readonly Action DoNothingAction = () => { };
            public static readonly Func<bool> TrueFunc = () => true;
            public static readonly Func<bool> FalseFunc = () => true;
            public static readonly Func<Task> CompletedTaskFunc = () => Task.CompletedTask;
        }

        public static class With<TArg1>
        {
            public static readonly Action<TArg1> DoNothingAction = (arg1) => { };
            public static readonly Predicate<TArg1> TruePredicate = (arg1) => true;
            public static readonly Predicate<TArg1> FalsePredicate = (arg1) => false;
            public static readonly Func<TArg1, bool> TrueFunc = (arg1) => true;
            public static readonly Func<TArg1, bool> FalseFunc = (arg1) => false;
            public static readonly Func<TArg1, Task> CompletedTaskWithArg1Func = (arg1) => Task.FromResult(arg1);
            
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
            public static readonly Func<TArg1,TArg2, bool> TrueFunc = (arg1, arg2) => true;
            public static readonly Func<TArg1, TArg2, bool> FalseFunc = (arg1, arg2) => false;
            public static readonly Func<TArg1, TArg2, Task> CompletedTaskWithArg1Func = (arg1, arg2) => Task.FromResult(arg1);
            public static readonly Func<TArg1, TArg2, Task> CompletedTaskWithArg2Func = (arg1, arg2) => Task.FromResult(arg2);
        }
    }
}