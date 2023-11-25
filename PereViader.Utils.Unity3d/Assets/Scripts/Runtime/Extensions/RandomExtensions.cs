using UnityEngine;

using Random = System.Random;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class RandomExtensions
    {
        public static Vector2 NextVector2(this Random rnd, float minX, float maxX, float minY, float maxY)
        {
            float x = (float)rnd.NextDouble() * (maxX - minX) + minX;
            float y = (float)rnd.NextDouble() * (maxY - minY) + minY;
            return new Vector2(x, y);
        }
        
        public static Vector2 NextVector2(this Random rnd, Vector2 min, Vector2 max)
        {
            return NextVector2(rnd, min.x, max.x, min.y, max.y);
        }

        public static Vector3 NextVector3(this Random rnd, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            float x = (float)rnd.NextDouble() * (maxX - minX) + minX;
            float y = (float)rnd.NextDouble() * (maxY - minY) + minY;
            float z = (float)rnd.NextDouble() * (maxZ - minZ) + minZ;
            return new Vector3(x, y, z);
        }
        
        public static Vector3 NextVector3(this Random rnd, Vector3 min, Vector3 max)
        {
            return NextVector3(rnd, min.x, max.x, min.y, max.y, min.z, max.z);
        }
        
        public static Vector3 NextVector4(this Random rnd, float minX, float maxX, float minY, float maxY, float minZ, float maxZ, float minW, float maxW)
        {
            float x = (float)rnd.NextDouble() * (maxX - minX) + minX;
            float y = (float)rnd.NextDouble() * (maxY - minY) + minY;
            float z = (float)rnd.NextDouble() * (maxZ - minZ) + minZ;
            float w = (float)rnd.NextDouble() * (maxW - minW) + minW;
            return new Vector4(x, y, z, w);
        }
        
        public static Vector4 NextVector4(this Random rnd, Vector4 min, Vector4 max)
        {
            return NextVector4(rnd, min.x, max.x, min.y, max.y, min.z, max.z, min.w, max.w);
        }
        
        public static Vector2 NextPointOnCircle(this Random rnd, float radius)
        {
            float angle = (float)rnd.NextDouble() * 360;
            float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector2(x, y);
        }

        public static Vector3 NextPointOnSphere(this Random rnd, float radius)
        {
            float theta = (float)rnd.NextDouble() * 360;
            float phi = (float)rnd.NextDouble() * 180;
            float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
            float z = radius * Mathf.Cos(phi);
            return new Vector3(x, y, z);
        }

        public static Quaternion NextQuaternion(this Random rnd)
        {
            // Generate random values for quaternion components
            float x = (float)rnd.NextDouble() * 2f - 1f; // Range: -1 to 1
            float y = (float)rnd.NextDouble() * 2f - 1f; // Range: -1 to 1
            float z = (float)rnd.NextDouble() * 2f - 1f; // Range: -1 to 1
            float w = (float)rnd.NextDouble() * 2f - 1f; // Range: -1 to 1

            // Normalize the quaternion
            Vector4 randomQuaternion = new Vector4(x, y, z, w).normalized;

            return new Quaternion(randomQuaternion.x, randomQuaternion.y, randomQuaternion.z, randomQuaternion.w);
        }
    }
}