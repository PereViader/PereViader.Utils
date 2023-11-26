using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Extensions
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random rnd, double minValue, double maxValue)
        {
            return rnd.NextDouble() * (maxValue - minValue) + minValue;
        }
        
        public static double NextFloat(this Random rnd)
        {
            return (float)rnd.NextDouble();
        }
        
        public static double NextFloat(this Random rnd, float minValue, float maxValue)
        {
            return rnd.NextFloat() * (maxValue - minValue) + minValue;
        }
        
        public static int NextInt(this Random rnd, int minValue, int maxValueExclusive)
        {
            return rnd.Next(minValue, maxValueExclusive);
        }
        
        public static int NextIntInclusive(this Random rnd, int minValue, int maxValueInclusive)
        {
            return rnd.Next(minValue, maxValueInclusive + 1);
        }
        
        public static bool NextBool(this Random rnd)
        {
            return rnd.NextDouble() >= 0.5d;
        }
        
        public static bool NextBool(this Random rnd, double weightOfTrue)
        {
            return rnd.NextDouble() < weightOfTrue;
        }
        
        public static T NextItem<T>(this Random rnd, IReadOnlyList<T> list)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("Asked for a random NextItem but List is empty", nameof(list));
            }
            
            return list[rnd.Next(list.Count)];
        }
        
        public static T NextItemWeighted<T>(this Random rnd, IReadOnlyList<T> list, Func<T, double> weightSelector)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("Asked for a random NextItem but List is empty", nameof(list));
            }
            
            return list.WeightedRandom(weightSelector, rnd);
        }
        
        public static void Shuffle<T>(this Random rnd, IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rnd.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static int NextSign(this Random rnd)
        {
            return rnd.NextBool() ? 1 : -1;
        }
    }
}