using System;

namespace PereViader.Utils.Common.Extensions
{
    public static class MathExtensions
    {
        public static int Fibonacci(int n)
        {
            var current = 0;
            var next = 1;
            for (var i = 0; i < n; i++)
            {
                var temp = current;
                current = next;
                next += temp;
            }
            return current;
        }
        
        public static double AreaOfTriangle(double baseLength, double height)
        {
            return 0.5d * baseLength * height;
        }
        
        public static double AreaOfTriangle(double a, double b, double c)
        {
            var s = (a + b + c) / 2;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }
        
        public static double AreaOfCircle(double radius)
        {
            return Math.PI * radius * radius;
        }
        
        public static double AreaOfSquare(double sideLength)
        {
            return sideLength * sideLength;
        }
        
        public static double MapValueFromRangeToRange(double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
        
        
        public static double HeightOfTriangle(double a, double b, double c)
        {
            var area = AreaOfTriangle(a, b, c);

            // Calculate and return the height with respect to base 'a'
            return (2 * area) / a;
        }
    }
}