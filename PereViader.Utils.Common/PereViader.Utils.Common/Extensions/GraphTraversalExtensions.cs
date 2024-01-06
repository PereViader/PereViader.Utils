using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Extensions
{
    public static class GraphTraversalExtensions
    {
        /// <summary>
        /// Performs an unrestricted depth-first traversal starting from the specified value.
        /// </summary>
        /// <typeparam name="T">The type of elements being traversed.</typeparam>
        /// <param name="value">The starting value for the traversal.</param>
        /// <param name="getNextValuesFunc">A function that provides the next values to explore.</param>
        /// <returns>An IEnumerable of elements in the order they are visited.</returns>
        public static IEnumerable<T> DepthFirstUnrestricted<T>(T value, Func<T, IEnumerable<T>> getNextValuesFunc)
        {
            var stack = new Stack<T>();
            stack.Push(value);

            while (stack.Count > 0)
            {
                var currentValue = stack.Pop();
                yield return currentValue;

                var nextValues = getNextValuesFunc(currentValue);

                foreach (var nextValue in nextValues)
                {
                    stack.Push(nextValue);
                }
            }
        }

        /// <summary>
        /// Performs a cycle-aware depth-first traversal starting from the specified value.
        /// </summary>
        /// <typeparam name="T">The type of elements being traversed.</typeparam>
        /// <param name="value">The starting value for the traversal.</param>
        /// <param name="getNextValuesFunc">A function that provides the next values to explore.</param>
        /// <returns>An IEnumerable of elements in the order they are visited.</returns>
        public static IEnumerable<T> DepthFirstCycleAware<T>(T value, Func<T, IEnumerable<T>> getNextValuesFunc)
        {
            var visited = new HashSet<T>();
            var stack = new Stack<T>();
            stack.Push(value);

            while (stack.Count > 0)
            {
                var currentValue = stack.Pop();

                yield return currentValue;
                visited.Add(currentValue);

                var nextValues = getNextValuesFunc(currentValue);

                foreach (var nextValue in nextValues)
                {
                    if (!visited.Contains(nextValue))
                    {
                        stack.Push(nextValue);
                    }
                }
            }
        }
        
        /// <summary>
        /// Performs an unrestricted breadth-first traversal starting from the specified value.
        /// </summary>
        /// <typeparam name="T">The type of elements being traversed.</typeparam>
        /// <param name="start">The starting value for the traversal.</param>
        /// <param name="getNextNodesFunc">A function that provides the next nodes to explore.</param>
        /// <returns>An IEnumerable of elements in the order they are visited during the traversal.</returns>
        public static IEnumerable<T> BreadthFirstUnrestricted<T>(T start, Func<T, IEnumerable<T>> getNextNodesFunc)
        {
            var queue = new Queue<T>();

            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                yield return current;

                var neighbors = getNextNodesFunc(current);

                foreach (var neighbor in neighbors)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }
        
        /// <summary>
        /// Performs a cycle-aware breadth-first traversal starting from the specified value.
        /// </summary>
        /// <typeparam name="T">The type of elements being traversed.</typeparam>
        /// <param name="start">The starting value for the traversal.</param>
        /// <param name="getNextNodesFunc">A function that provides the next nodes to explore.</param>
        /// <returns>An IEnumerable of elements in the order they are visited during the traversal.</returns>
        public static IEnumerable<T> BreadthFirstCycleAware<T>(T start, Func<T, IEnumerable<T>> getNextNodesFunc)
        {
            var visited = new HashSet<T>();
            var queue = new Queue<T>();

            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                
                yield return current;
                visited.Add(current);

                var neighbors = getNextNodesFunc(current);

                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
    }
}