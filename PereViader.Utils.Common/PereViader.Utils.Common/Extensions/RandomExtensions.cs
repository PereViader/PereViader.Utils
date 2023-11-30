using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Extensions
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }
        
        public static double NextFloat(this Random random)
        {
            return (float)random.NextDouble();
        }
        
        public static double NextFloat(this Random random, float minValue, float maxValue)
        {
            return random.NextFloat() * (maxValue - minValue) + minValue;
        }
        
        public static int NextInt(this Random random, int minValue, int maxValueExclusive)
        {
            return random.Next(minValue, maxValueExclusive);
        }
        
        public static int NextIntInclusive(this Random random, int minValue, int maxValueInclusive)
        {
            return random.Next(minValue, maxValueInclusive + 1);
        }
        
        public static bool NextBool(this Random random)
        {
            return random.NextDouble() >= 0.5d;
        }
        
        public static bool NextBool(this Random random, double weightOfTrue)
        {
            return random.NextDouble() < weightOfTrue;
        }
        
        public static T NextItem<T>(this Random random, IReadOnlyList<T> list)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("Asked for a random NextItem but List is empty", nameof(list));
            }

            var index = random.Next(list.Count);
            return list[index];
        }
        
        public static T NextItemWeighted<T>(this Random random, IReadOnlyList<T> list, Func<T, double> weightSelector)
        {
            if (list.Count == 0)
            {
                throw new ArgumentException("Asked for a random NextItem but List is empty", nameof(list));
            }

            var totalWeight = 0d;
            for (var index = 0; index < list.Count; index++)
            {
                totalWeight += weightSelector(list[index]);
            }

            var randomWeight = random.NextDouble() * totalWeight;
            var cumulativeWeight = 0d;

            for (var index = 0; index < list.Count; index++)
            {
                var item = list[index];
                cumulativeWeight += weightSelector(item);
                if (cumulativeWeight >= randomWeight)
                {
                    return item;
                }
            }

            throw new Exception("Could not take a random a random element from the list");
        }
        
        public static void Shuffle<T>(this Random random, IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static int NextSign(this Random random)
        {
            return random.NextBool() ? 1 : -1;
        }
    }
}