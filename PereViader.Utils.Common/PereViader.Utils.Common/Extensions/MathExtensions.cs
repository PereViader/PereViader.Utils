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
        
        public static double HypotenuseOfTriangle(double a, double b)
        {
            return Math.Sqrt(a * a + b * b);
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
            return (2 * area) / a;
        }
        
        public static double Normalize(double value, double max)
        {
            return DivideSafe(value, max);
        }
        
        public static double NormalizeClamped(double value, double max)
        {
            value = Math.Clamp(value, 0, max);

            return Normalize(value, max);
        }
        
        public static float Normalize(float value, float max)
        {
            return DivideSafe(value, max);
        }
        
        public static float NormalizeClamped(float value, float max)
        {
            value = Math.Clamp(value, 0, max);
            
            return Normalize(value, max);
        }

        private static double Normalize(double value, double min, double max)
        {
            var rebasedMax = max - min;
            var rebasedValue = value - min;

            return DivideSafe(rebasedValue, rebasedMax);
        }
        
        public static double NormalizeClamped(double value, double min, double max)
        {
            value = Math.Clamp(value, min, max);

            return Normalize(value, min, max);
        }

        public static float NormalizeClamped(float value, float min, float max)
        {
            value = Math.Clamp(value, min, max);

            return Normalize(value, min, max);
        }

        private static float Normalize(float value, float min, float max)
        {
            var rebasedMax = max - min;
            var rebasedValue = value - min;

            return DivideSafe(rebasedValue, rebasedMax);
        }

        /// <summary>
        /// Normalizes a given value between min and max, but taking into account a window that must be visible at all times.
        /// Because the window must be within bounds of min/max at all times, thus making it visible, the regular normalized value might need to be shifted
        /// </summary>
        /// <param name="value">The value to be normalized [min, max].</param>
        /// <param name="min">The minimum possible value [-inf, max].</param>
        /// <param name="max">The maximum possible value [min, +inf].</param>
        /// <param name="windowSize">The size of the window [0, inf].</param>
        /// <param name="normalizedPositionWithinWindow">The normalized position within the window [0,1].</param>
        /// <returns>The clamped normalized value that takes into account the window size and position within the window [0,1].</returns>
        /// <remarks>
        /// Think of a ScrollRect in Unity3d where the content of the scroll rect might be larger than the visible area. The viewport size is the window.
        /// </remarks>
        public static float NormalizeClampedInWindow( 
            float value,
            float min,
            float max,
            float windowSize,
            float normalizedPositionWithinWindow)
        {
            var viewportDisplacement = windowSize * normalizedPositionWithinWindow;
            var desiredPosition = value - viewportDisplacement;

            return NormalizeClamped(desiredPosition, min, max);
        }
        
        public static double DivideSafe(double a, double b)
        {
            return b == 0d ? 0f : a / b;
        }
        
        public static float DivideSafe(float a, float b)
        {
            return b == 0f ? 0f : a / b;
        }
        
        public static int DivideSafe(int a, int b)
        {
            return b == 0 ? 0 : a / b;
        }
    }
}