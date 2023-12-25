using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class Vector2Extensions
    {
        public static Vector3 ToVector3XY(this Vector2 v, float z = 0f) => new Vector3(v.x, v.y, z);
        public static Vector3 ToVector3XZ(this Vector2 v, float y = 0f) => new Vector3(v.x, y, v.y);
        public static Vector3 ToVector3YX(this Vector2 v, float z = 0f) => new Vector3(v.y, v.x, z);
        public static Vector3 ToVector3YZ(this Vector2 v, float x = 0f) => new Vector3(x, v.x, v.y);
        public static Vector3 ToVector3ZX(this Vector2 v, float y = 0f) => new Vector3(v.y, y, v.x);
        public static Vector3 ToVector3ZY(this Vector2 v, float x = 0f) => new Vector3(x, v.y, v.x);

        public static Vector2Int ToVector2IntTruncated(this Vector2 v) => new Vector2Int((int)v.x, (int)v.y);
        public static Vector2Int ToVector2IntRounded(this Vector2 v) => new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }
}