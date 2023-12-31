using System.Collections.Generic;
using PereViader.Utils.Common.Optionals;

namespace PereViader.Utils.Common.Extensions
{
    public static class ValueExtensions
    {
        public static T? ToNullableNullOnDefaultValue<T>(this T value) where T : struct
        {
            return EqualityComparer<T>.Default.Equals(value, default) ? null : value;
        }
        
        public static Optional<T> ToOptionalNoneOnDefaultValue<T>(this T value) where T : struct
        {
            return EqualityComparer<T>.Default.Equals(value, default) ? Optional<T>.None : Optional<T>.Some(value);
        }
    }
}