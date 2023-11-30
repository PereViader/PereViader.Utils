using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.Extensions
{
    public static class DelegateExtensions
    {
        public static class With
        {
            public static readonly Action DoNothing = () => { };
            public static readonly Func<bool> ReturnTrue = () => true;
            public static readonly Func<bool> ReturnFalse = () => true;
            public static readonly Func<Task> ReturnCompletedTask = () => Task.CompletedTask;
        }

        public static class With<TArg1>
        {
            public static readonly Action<TArg1> DoNothing = (arg1) => { };
            public static readonly Func<TArg1, bool> ReturnTrue = (arg1) => true;
            public static readonly Func<TArg1, bool> ReturnFalse = (arg1) => false;
            public static readonly Func<TArg1, Task> ReturnCompletedTaskWithArg1 = (arg1) => Task.FromResult(arg1);
        }
        
        public static class With<TArg1, TArg2>
        {
            public static readonly Action<TArg1, TArg2> DoNothing = (arg1, arg2) => { };
            public static readonly Func<TArg1,TArg2, bool> ReturnTrue = (arg1, arg2) => true;
            public static readonly Func<TArg1, TArg2, bool> ReturnFalse = (arg1, arg2) => false;
            public static readonly Func<TArg1, TArg2, Task> ReturnCompletedTaskWithArg1 = (arg1, arg2) => Task.FromResult(arg1);
            public static readonly Func<TArg1, TArg2, Task> ReturnCompletedTaskWithArg2 = (arg1, arg2) => Task.FromResult(arg2);
        }
    }
}