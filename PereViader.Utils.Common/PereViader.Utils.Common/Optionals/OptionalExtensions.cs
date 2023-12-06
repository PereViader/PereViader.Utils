using System;

namespace PereViader.Utils.Common.Optionals
{
    public static class OptionalExtensions
    {
        public static TResult Match<T, TResult>(this Optional<T> optional, Func<T, TResult> whenSome, Func<TResult> whenNone)
        {
            if (optional.TryGetValue(out var value))
            {
                return whenSome(value);
            }
            
            return whenNone();
        }
        
        public static void Match<T, TResult>(this Optional<T> optional, Action<T> whenSome, Action whenNone)
        {
            if (optional.TryGetValue(out var value))
            {
                whenSome(value);
                return;
            }
            
            whenNone();
        }

        public static Optional<TResult> MapSome<T, TResult>(this Optional<T> optional, Func<T, TResult> func)
        {
            return optional.TryGetValue(out var value) 
                ? Optional<TResult>.Some(func(value)) 
                : Optional<TResult>.None;
        }
        
        public static Optional<TResult> MapSome<T, TResult>(this Optional<T> optional, Func<T, Optional<TResult>> func)
        {
            return optional.TryGetValue(out var value) 
                ? func(value) 
                : Optional<TResult>.None;
        }

        public static Optional<T> MapNone<T>(this Optional<T> optional, Func<T> func)
        {
            return optional.HasValue 
                ? optional 
                : Optional<T>.Some(func.Invoke());
        }
        
        public static Optional<T> MapNone<T>(this Optional<T> optional, Func<Optional<T>> func)
        {
            return optional.HasValue 
                ? optional 
                : func.Invoke();
        }
        
        public static T GetValueOrDefault<T>(this Optional<T> optional, T defaulValue = default)
        {
            if (optional.TryGetValue(out var value))
            {
                return value;
            }
            
            return defaulValue;
        }
        
        public static T GetValueOrDefault<T>(this Optional<T> optional, Func<T> func)
        {
            if (optional.TryGetValue(out var value))
            {
                return value;
            }
            
            return func();
        }
    }
}