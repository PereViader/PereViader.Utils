using System;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.Optionals
{
    public static class OptionalExtensions
    {
        public static T? GetNullableValue<T>(Optional<T> optional) where T : struct => 
            optional.TryGetValue(out var value).ToNullableValue(value);

        public static T? GetNullable<T>(Optional<T> optional) where T : class =>
            optional.TryGetValue(out var value).ToNullable(value);

        public static TResult Match<T, TResult>(this Optional<T> optional, Func<T, TResult> whenSome, Func<TResult> whenNone) => 
            optional.TryGetValue(out var value) ? whenSome(value) : whenNone();

        public static void Match<T, TResult>(this Optional<T> optional, Action<T> whenSome, Action whenNone)
        {
            if (optional.TryGetValue(out var value))
            {
                whenSome(value);
                return;
            }
            
            whenNone();
        }

        public static Optional<TResult> MapSome<T, TResult>(this Optional<T> optional, Func<T, TResult> func) =>
            optional.TryGetValue(out var value) 
                ? Optional<TResult>.Some(func(value)) 
                : Optional<TResult>.None;

        public static Optional<TResult> MapSome<T, TResult>(this Optional<T> optional, Func<T, Optional<TResult>> func) =>
            optional.TryGetValue(out var value) 
                ? func(value) 
                : Optional<TResult>.None;

        public static Optional<T> MapNone<T>(this Optional<T> optional, Func<T> func) =>
            optional.HasValue 
                ? optional 
                : Optional<T>.Some(func.Invoke());

        public static Optional<T> MapNone<T>(this Optional<T> optional, Func<Optional<T>> func) =>
            optional.HasValue 
                ? optional 
                : func.Invoke();

        public static T GetValueOrDefault<T>(this Optional<T> optional, T defaultValue = default!) => 
            optional.TryGetValue(out var value) ? value : defaultValue;

        public static T GetValueOrDefault<T>(this Optional<T> optional, Func<T> func) => 
            optional.TryGetValue(out var value) ? value : func();
    }
}