using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class Vector3Extensions 
    {
        public static Vector2 ToVector2XY(this Vector3 v) => new Vector2(v.x, v.y);
        public static Vector2 ToVector2XZ(this Vector3 v) => new Vector2(v.x, v.z);
        public static Vector2 ToVector2YX(this Vector3 v) => new Vector2(v.y, v.x);
        public static Vector2 ToVector2YZ(this Vector3 v) => new Vector2(v.y, v.z);
        public static Vector2 ToVector2ZX(this Vector3 v) => new Vector2(v.z, v.x);
        public static Vector2 ToVector2ZY(this Vector3 v) => new Vector2(v.z, v.y);
        
        public static Vector3Int ToVector3IntRounded(this Vector3 v) => new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        public static Vector3Int ToVector3IntTruncated(this Vector3 v) => new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }
}