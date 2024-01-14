#nullable enable

using PereViader.Utils.Common.Optionals;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class UnityEngineObjectExtensions
    {
        public static T? Null<T>(this T obj) where T : UnityEngine.Object
        {
            return obj ? obj : null;
        }

        public static Optional<T> ToOptional<T>(this T obj) where T : UnityEngine.Object
        {
            return Optional<T>.Maybe(obj, obj != null);
        }
    }
}