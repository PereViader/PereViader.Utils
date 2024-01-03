using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Extensions
{
    public static class ReadOnlyListExtensions
    {
        public static T? Find<T>(this IReadOnlyList<T> list, Func<T, bool> match)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (match(element))
                {
                    return element;
                }
            }

            return default;
        }

        public static T? Find<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> match, TArg arg)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (match(element, arg))
                {
                    return element;
                }
            }

            return default;
        }
        
        public static List<T> FindAll<T>(this IReadOnlyList<T> list, Func<T, bool> match, List<T> result)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (match(element))
                {
                    result.Add(element);
                }
            }

            return result;
        }
        
        public static List<T> FindAll<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> match, TArg arg, List<T> result)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (match(element, arg))
                {
                    result.Add(element);
                }
            }

            return result;
        }
        
        public static List<TResult> ConvertAll<TSource, TResult>(this IReadOnlyList<TSource> list, Func<TSource, TResult> func, List<TResult> result)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i];
                var elementRes = func.Invoke(element);
                result.Add(elementRes);
            }

            return result;
        }
        
        public static List<TResult> ConvertAll<TSource, TArg, TResult>(this IReadOnlyList<TSource> list, Func<TSource, TArg, TResult> func, TArg arg, List<TResult> result)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var element = list[i];
                var elementRes = func.Invoke(element, arg);
                result.Add(elementRes);
            }

            return result;
        }
        
        public static bool Exists<T>(this IReadOnlyList<T> list, Func<T, bool> match)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    return true;
                }
            }

            return false;
        }
        
        public static bool Exists<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> match, TArg arg)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i], arg))
                {
                    return true;
                }
            }

            return false;
        }
        
        public static int FindIndex<T>(this IReadOnlyList<T> list, Func<T, bool> match)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindIndex<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> match, TArg arg)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i], arg))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindIndex<T>(this IReadOnlyList<T> list, int startIndex, Func<T, bool> match)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, "startIndex is out of range.");
            }
            
            for (int i = startIndex; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindIndex<T, TArg>(this IReadOnlyList<T> list, int startIndex, Func<T, TArg, bool> match, TArg arg)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex, "startIndex is out of range.");
            }
            
            for (int i = startIndex; i < list.Count; i++)
            {
                if (match(list[i], arg))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindIndex<T>(this IReadOnlyList<T> list, int startIndex, int count, Func<T, bool> match)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            if (count < 0 || startIndex + count > list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "count is out of range.");
            }

            int endIndex = startIndex + count;

            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindIndex<T, TArg>(this IReadOnlyList<T> list, int startIndex, int count, Func<T, TArg, bool> match, TArg arg)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            if (count < 0 || startIndex + count > list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "count is out of range.");
            }

            int endIndex = startIndex + count;

            for (int i = startIndex; i < endIndex; i++)
            {
                if (match(list[i], arg))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindLastIndex<T>(this IReadOnlyList<T> list, int startIndex, int count, Func<T, bool> match)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            if (count < 0 || startIndex - count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "count is out of range.");
            }

            int endIndex = startIndex - count;

            for (int i = startIndex; i >= endIndex; i--)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindLastIndex<T, TArg>(this IReadOnlyList<T> list, int startIndex, int count, Func<T, TArg, bool> match, TArg arg)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            if (count < 0 || startIndex - count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "count is out of range.");
            }

            int endIndex = startIndex - count;

            for (int i = startIndex; i >= endIndex; i--)
            {
                if (match(list[i], arg))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindLastIndex<T>(this IReadOnlyList<T> list, int startIndex, Func<T, bool> match)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            for (int i = startIndex; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindLastIndex<T, TArg>(this IReadOnlyList<T> list, int startIndex, Func<T, TArg, bool> match, TArg arg)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            for (int i = startIndex; i >= 0; i--)
            {
                if (match(list[i], arg))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindLastIndex<T>(this IReadOnlyList<T> list, Func<T, bool> match)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }
        
        public static int FindLastIndex<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> match, TArg arg)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (match(list[i], arg))
                {
                    return i;
                }
            }

            return -1;
        }

        public static T? FindLast<T>(this IReadOnlyList<T> list, int startIndex, int count, Func<T, bool> match)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            if (count < 0 || startIndex - count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "count is out of range.");
            }

            int endIndex = startIndex - count;

            for (int i = startIndex; i >= endIndex; i--)
            {
                var value = list[i];
                if (match(value))
                {
                    return value;
                }
            }

            return default;
        }
        
        public static T? FindLast<T, TArg>(this IReadOnlyList<T> list, int startIndex, int count, Func<T, TArg, bool> match, TArg arg)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            if (count < 0 || startIndex - count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "count is out of range.");
            }

            int endIndex = startIndex - count;

            for (int i = startIndex; i >= endIndex; i--)
            {
                var value = list[i];
                if (match(value, arg))
                {
                    return value;
                }
            }

            return default;
        }
        
        public static T? FindLast<T>(this IReadOnlyList<T> list, int startIndex, Func<T, bool> match)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            for (int i = startIndex; i >= 0; i--)
            {
                var value = list[i];
                if (match(value))
                {
                    return value;
                }
            }

            return default;
        }
        
        public static T? FindLast<T, TArg>(this IReadOnlyList<T> list, int startIndex, Func<T, TArg, bool> match, TArg arg)
        {
            if (startIndex < 0 || startIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,"startIndex is out of range.");
            }

            for (int i = startIndex; i >= 0; i--)
            {
                var value = list[i];
                if (match(value, arg))
                {
                    return value;
                }
            }

            return default;
        }
        
        public static T? FindLast<T>(this IReadOnlyList<T> list, Func<T, bool> match)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var value = list[i];
                if (match(value))
                {
                    return value;
                }
            }

            return default;
        }
        
        public static T? FindLast<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> match, TArg arg)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var value = list[i];
                if (match(value, arg))
                {
                    return value;
                }
            }

            return default;
        }
        
        public static bool TrueForAll<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!predicate(list[i]))
                {
                    return false;
                }
            }

            return true;
        }
        
        public static bool TrueForAll<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> predicate, TArg arg)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!predicate(list[i], arg))
                {
                    return false;
                }
            }

            return true;
        }
        
        public static bool TrueForAny<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return true;
                }
            }

            return false;
        }
        
        public static bool TrueForAny<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> predicate, TArg arg)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i], arg))
                {
                    return true;
                }
            }

            return false;
        }
        
        public static bool FalseForAny<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            return !list.TrueForAll(predicate);
        }
        
        public static bool FalseForAny<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> predicate, TArg arg)
        {
            return !list.TrueForAll(predicate, arg);
        }
        
        public static bool FalseForAll<T>(this IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            return !list.TrueForAny(predicate);
        }
        
        public static bool FalseForAll<T, TArg>(this IReadOnlyList<T> list, Func<T, TArg, bool> predicate, TArg arg)
        {
            return !list.TrueForAny(predicate, arg);
        }
    }
}