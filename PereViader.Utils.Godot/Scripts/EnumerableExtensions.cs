using System.Collections.Generic;

namespace PereViader.Utils.Godot;

public static class EnumerableExtensions
{
    public static IEnumerable<(T value, int index)> ZipIndex<T>(this IEnumerable<T> enumerable)
    {
        int index = 0;
        foreach (var value in enumerable)
        {
            yield return (value, index);
            index++;
        }
    }
}