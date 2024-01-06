using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace PereViader.Utils.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
        {
            return source as IReadOnlyList<T> ?? source.ToArray();
        }
        
        public static List<TResult> ConvertAll<T, TResult>(this IEnumerable<T> source, Func<T, TResult> func, List<TResult> results)
        {
            foreach (var element in source)
            {
                var result = func(element);
                results.Add(result);
            }

            return results;
        }
        
        public static List<TResult> ConvertAll<T, TArg, TResult>(this IEnumerable<T> source, Func<T, TArg, TResult> func, TArg arg, List<TResult> results)
        {
            foreach (var element in source)
            {
                var result = func(element, arg);
                results.Add(result);
            }

            return results;
        }
        
        public static IEnumerable<TResult> Select<T, TArg, TResult>(this IEnumerable<T> source, Func<T, TArg, TResult> func, TArg arg)
        {
            foreach (var element in source)
            {
                var result = func(element, arg);
                yield return result;
            }
        }
        
        public static List<T> FindAll<T>(this IEnumerable<T> enumerable, Func<T, bool> match, List<T> result)
        {
            foreach (var element in enumerable)    
            {
                if (match(element))
                {
                    result.Add(element);
                }
            }

            return result;
        }
        
        public static List<T> FindAll<T, TArg>(this IEnumerable<T> enumerable, Func<T, TArg, bool> match, TArg arg, List<T> result)
        {
            foreach (var element in enumerable)    
            {
                if (match(element, arg))
                {
                    result.Add(element);
                }
            }

            return result;
        }

        public static IEnumerable<T> Where<T, TArg>(this IEnumerable<T> source, Func<T, TArg, bool> predicate, TArg arg)
        {
            foreach (var element in source)
            {
                if (predicate.Invoke(element, arg))
                {
                    yield return element;
                }
            }
        }
        
        public static bool Any<T, TArg>(this IEnumerable<T> source, Func<T, TArg, bool> predicate, TArg arg)
        {
            foreach (var element in source)
            {
                if (predicate.Invoke(element, arg))
                {
                    return true;
                }
            }

            return false;
        }
        
        public static bool All<T, TArg>(this IEnumerable<T> source, Func<T, TArg, bool> predicate, TArg arg)
        {
            foreach (var element in source)
            {
                if (!predicate.Invoke(element, arg))
                {
                    return false;
                }
            }

            return true;
        }
        
        public static T First<T, TArg>(this IEnumerable<T> source, Func<T, TArg, bool> predicate, TArg arg)
        {
            if (!TryGetFirst(source, predicate, arg, out var value))
            {
                throw new InvalidOperationException("There was no element that satisfied the predicate");
            }

            return value;
        }
        
        public static int Count<T, TArg>(this IEnumerable<T> source, Func<T, TArg, bool> predicate, TArg arg)
        {
            var count = 0;
            foreach (var element in source)
            {
                if (predicate.Invoke(element, arg))
                {
                    count++;
                }
            }

            return count;
        }
        
        public static bool TryGetFirst<T>(this IEnumerable<T> source, out T value)
        {
            foreach (var element in source)
            {
                value = element;
                return true;
            }

            value = default!;
            return false;
        }
        
        public static bool TryGetFirst<T, TArg>(this IEnumerable<T> source, Func<T, TArg, bool> predicate, TArg arg, out T value)
        {
            foreach (var element in source)
            {
                if (predicate.Invoke(element, arg))
                {
                    value = element;
                    return true;
                }
            }

            value = default!;
            return false;
        }
        
        public static IEnumerable<TSource> SkipWhile<TSource, TArg>(
            this IEnumerable<TSource> source,
            Func<TSource, TArg, bool> predicate,
            TArg argument
        )
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TSource element = enumerator.Current;

                if (!predicate(element, argument))
                {
                    yield return element;
                    break;
                }
            }

            while (enumerator.MoveNext())
            {
                TSource element = enumerator.Current;
                yield return element;
            }
        }
        
        public static IEnumerable<TSource> SkipUntil<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TSource element = enumerator.Current;

                if (predicate(element))
                {
                    yield return element;
                    break;
                }
            }

            while (enumerator.MoveNext())
            {
                TSource element = enumerator.Current;
                yield return element;
            }
        }
        
        public static IEnumerable<TSource> SkipUntil<TSource, TArg>(
            this IEnumerable<TSource> source,
            Func<TSource, TArg, bool> predicate,
            TArg argument
        )
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TSource element = enumerator.Current;

                if (predicate(element, argument))
                {
                    yield return element;
                    break;
                }
            }

            while (enumerator.MoveNext())
            {
                TSource element = enumerator.Current;
                yield return element;
            }
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
        
        public static IEnumerable<T> TakeRandomElementsRepeating<T>(this IEnumerable<T> source, int count, Random random)
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
        
        public static void ForEach<T, TArg>(this IEnumerable<T> source, Action<T, TArg> action, TArg arg)
        {
            foreach (var element in source)
            {
                action(element, arg);
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
        
        public delegate bool SelectWhereDelegate<in T, in TArg, TResult>(T value, TArg arg, out TResult result);
        
        public static IEnumerable<TResult> SelectWhere<T, TArg, TResult>(this IEnumerable<T> source, SelectWhereDelegate<T, TArg, TResult> selector, TArg arg)
        {
            foreach (var element in source)
            {
                if (selector(element, arg, out var result))
                {
                    yield return result;
                }
            }
        }
        
        public delegate bool SelectWhereIndexedDelegate<in T, TResult>(T value, int index, out TResult result);

        public static IEnumerable<TResult> SelectWhere<T, TResult>(this IEnumerable<T> source, SelectWhereIndexedDelegate<T, TResult> selector)
        {
            var index = 0;
            foreach (var element in source)
            {
                if (selector(element, index, out var result))
                {
                    yield return result;
                }

                index++;
            }
        }
        
        public delegate bool SelectWhereIndexedDelegate<in T, in TArg, TResult>(T value, TArg arg, int index, out TResult result);

        public static IEnumerable<TResult> SelectWhere<T, TArg, TResult>(this IEnumerable<T> source, SelectWhereIndexedDelegate<T, TArg, TResult> selector, TArg arg)
        {
            var index = 0;
            foreach (var element in source)
            {
                if (selector(element, arg, index, out var result))
                {
                    yield return result;
                }

                index++;
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
        
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
        
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> equalityComparer)
        {
            return new HashSet<T>(source, equalityComparer);
        }
    }
}