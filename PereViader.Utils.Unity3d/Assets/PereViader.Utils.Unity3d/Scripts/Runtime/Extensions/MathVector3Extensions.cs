using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class Vector3MathExtensions
    {
        public static Vector3 PlaneNormal(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 dir = Vector3.Cross(b - a, c - a);
            return Vector3.Normalize(dir);
        }
        
        public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            var u = 1 - t;
            var tt = t * t;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * t;

            Vector3 p = uuu * p0; //first term
            p += 3 * uu * t * p1; //second term
            p += 3 * u * tt * p2; //third term
            p += ttt * p3; //fourth term

            return p;
        }

        public static Vector3 ClosestPointOnSegment(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 lineDirection = lineEnd - lineStart;
            float lineLengthSquared = lineDirection.sqrMagnitude;
            if (lineLengthSquared == 0.0f)
                return lineStart; // The segment is a point

            // Project point onto the line segment and clamp it to the segment's bounds
            float t = Mathf.Clamp01(Vector3.Dot(point - lineStart, lineDirection) / lineLengthSquared);
            return lineStart + t * lineDirection;
        }

        public static Vector3 ClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 lineDirection = (lineEnd - lineStart).normalized;
            float dotProduct = Vector3.Dot(point - lineStart, lineDirection);
            return lineStart + dotProduct * lineDirection;
        }
    }
}