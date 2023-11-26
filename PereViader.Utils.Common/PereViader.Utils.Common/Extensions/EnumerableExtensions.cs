using System;
using System.Collections.Generic;
using System.Linq;

namespace PereViader.Utils.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
        {
            return source as IReadOnlyList<T> ?? source.ToArray();
        }
        
        public static IEnumerable<T> TakeRandomNonRepeating<T>(this IEnumerable<T> source, int count, Random random)
        {
            if (count <= 0)
            {
                yield break;
            }
            
            var array = source.ToArray();
            var sourceCount = array.Length;
            var elements = Math.Min(count, sourceCount);

            for (var i = 0; i < elements; i++)
            {
                var index = random.Next(0, sourceCount);
                yield return array[index];
                array[index] = array[sourceCount - 1];
                sourceCount--;
            }
        }
        
        public static IEnumerable<T> RandomElementsRepeating<T>(this IEnumerable<T> source, int count, Random random)
        {
            if (count <= 0)
            {
                yield break;
            }
            
            var readOnlyList = source.ToReadOnlyList();
            var sourceCount = readOnlyList.Count;

            for (var i = 0; i < count; i++)
            {
                var index = random.Next(sourceCount);
                yield return readOnlyList[index];
            }
        }
        
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {
            var elements = source.ToArray();
            for (var i = elements.Length - 1; i > 0; i--)
            {
                var swapIndex = random.Next(i + 1);
                (elements[i], elements[swapIndex]) = (elements[swapIndex], elements[i]);
            }
            return elements;
        }
        
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var element in source)
            {
                action(element);
            }
        }

        public delegate bool SelectWhereDelegate<in T, TResult>(T value, out TResult result);
        
        public static IEnumerable<TResult> SelectWhere<T, TResult>(this IEnumerable<T> source, SelectWhereDelegate<T, TResult> selector)
        {
            foreach (var element in source)
            {
                if (selector(element, out var result))
                {
                    yield return result;
                }
            }
        }
        
        public static IEnumerable<(T item, int index)> ZipIndex<T>(this IEnumerable<T> source)
        {
            int index = 0;
            foreach (var item in source)
            {
                yield return (item, index);
                index++;
            }
        }

        public static T WeightedRandom<T>(this IEnumerable<T> enumerable, Func<T, double> weightSelector, Random random)
        {
            var readOnlyList = enumerable.ToReadOnlyList();
            if (readOnlyList.Count == 0)
            {
                throw new InvalidOperationException("The enumerable is empty, thus it could not generate a weighted random");
            }

            var totalWeight = 0d;
            for (var index = 0; index < readOnlyList.Count; index++)
            {
                totalWeight += weightSelector(readOnlyList[index]);
            }

            var randomWeight = random.NextDouble() * totalWeight;
            var cumulativeWeight = 0d;

            for (var index = 0; index < readOnlyList.Count; index++)
            {
                var item = readOnlyList[index];
                cumulativeWeight += weightSelector(item);
                if (cumulativeWeight >= randomWeight)
                {
                    return item;
                }
            }

            throw new Exception("Could not take a random a random element from the list");
        }
    }
}