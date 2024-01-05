using PereViader.Utils.Common.Optionals;

namespace PereViader.Utils.Common.Extensions
{
    public static class BoolExtensions
    {
        public static T? ToNullable<T>(this bool boolean, T value) where T : class
        {
            return boolean ? value : null;
        }
        
        public static T? ToNullableValue<T>(this bool boolean, T value) where T : struct
        {
            return boolean ? value : null;
        }

        public static Optional<T> ToOptional<T>(this bool boolean, T value)
        {
            return Optional<T>.Maybe(value, boolean);
        }
    }
}