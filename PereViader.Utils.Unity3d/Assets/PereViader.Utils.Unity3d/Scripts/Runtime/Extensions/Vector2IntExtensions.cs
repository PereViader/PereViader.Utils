using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class Vector2IntExtensions
    {
        public static Vector3Int ToVector3IntXY(this Vector2Int v, int z = 0) => new(v.x, v.y, z);
        public static Vector3Int ToVector3IntXZ(this Vector2Int v, int y = 0) => new(v.x, y, v.y);
        public static Vector3Int ToVector3IntYX(this Vector2Int v, int z = 0) => new(v.y, v.x, z);
        public static Vector3Int ToVector3IntYZ(this Vector2Int v, int x = 0) => new(x, v.x, v.y);
        public static Vector3Int ToVector3IntZX(this Vector2Int v, int y = 0) => new(v.y, y, v.x);
        public static Vector3Int ToVector3IntZY(this Vector2Int v, int x = 0) => new(x, v.y, v.x);
        
        public static Vector2 ToVector2(this Vector2Int v) => new(v.x, v.y);
    }
}