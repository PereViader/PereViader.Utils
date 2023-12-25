using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class TransformExtensions
    {
        public static void ResetPositionRotationScale(this Transform transform)
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;
        }
        
        public static void ResetPositionRotation(this Transform transform)
        {
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}