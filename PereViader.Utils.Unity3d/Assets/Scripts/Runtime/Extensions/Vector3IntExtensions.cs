using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class Vector3IntExtensions 
    {
        public static Vector2Int ToVector2IntXY(this Vector3Int v) => new Vector2Int(v.x, v.y);
        public static Vector2Int ToVector2IntXZ(this Vector3Int v) => new Vector2Int(v.x, v.z);
        public static Vector2Int ToVector2IntYX(this Vector3Int v) => new Vector2Int(v.y, v.x);
        public static Vector2Int ToVector2IntYZ(this Vector3Int v) => new Vector2Int(v.y, v.z);
        public static Vector2Int ToVector2IntZX(this Vector3Int v) => new Vector2Int(v.z, v.x);
        public static Vector2Int ToVector2IntZY(this Vector3Int v) => new Vector2Int(v.z, v.y);
        
        public static Vector3 ToVector3(this Vector3Int v) => v.ToVector3XYZ();
        public static Vector3 ToVector3XYZ(this Vector3Int v) => new Vector3(v.x, v.y, v.z);
        public static Vector3 ToVector3XZY(this Vector3Int v) => new Vector3(v.x, v.z, v.y);
        public static Vector3 ToVector3YXZ(this Vector3Int v) => new Vector3(v.y, v.x, v.z);
        public static Vector3 ToVector3YZX(this Vector3Int v) => new Vector3(v.y, v.z, v.x);
        public static Vector3 ToVector3ZXY(this Vector3Int v) => new Vector3(v.z, v.x, v.y);
        public static Vector3 ToVector3ZYX(this Vector3Int v) => new Vector3(v.z, v.y, v.x);
    }
}