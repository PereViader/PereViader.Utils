using PereViader.Utils.Common.Optionals;

namespace PereViader.Utils.Common.Extensions
{
    public static class NullableExtensions
    {
        public static Optional<T> ToOptional<T>(this T? value) where T : struct
        {
            return value.HasValue ? Optional<T>.Some(value.Value) : Optional<T>.None;
        }
        
        public static Optional<T> ToOptional<T>(this T? value) where T : class
        {
            //Some already does null validation. This is why we can use '!' 
            return Optional<T>.Some(value!);
        }
    }
}