using PereViader.Utils.Common.DiscriminatedUnions;
using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class MathVector2Extensions
    {
        public static Vector2? TryGetIntersectionPoint(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 s1 = a2 - a1;
            Vector2 s2 = b2 - b1;

            var s = (-s1.y * (a1.x - b1.x) + s1.x * (a1.y - b1.y)) / (-s2.x * s1.y + s1.x * s2.y);
            var t = ( s2.x * (a1.y - b1.y) - s2.y * (a1.x - b1.x)) / (-s2.x * s1.y + s1.x * s2.y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                return new Vector2(a1.x + (t * s1.x), a1.y + (t * s1.y));
            }

            return null;
        }
        
        public static Vector2 ClosestPointOnSegment(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            Vector2 lineDirection = lineEnd - lineStart;
            float lineLengthSquared = lineDirection.sqrMagnitude;
            if (lineLengthSquared == 0.0f)
                return lineStart; // The segment is a point

            // Project point onto the line segment and clamp it to the segment's bounds
            float t = Mathf.Clamp01(Vector2.Dot(point - lineStart, lineDirection) / lineLengthSquared);
            return lineStart + t * lineDirection;
        }

        
        public static Vector2 ClosestPointOnLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            Vector2 lineDirection = (lineEnd - lineStart).normalized;
            float dotProduct = Vector2.Dot(point - lineStart, lineDirection);
            return lineStart + dotProduct * lineDirection;
        }
    }
}