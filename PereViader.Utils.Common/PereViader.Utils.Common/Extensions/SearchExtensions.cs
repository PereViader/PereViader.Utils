using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.Extensions
{
    public static class SearchExtensions
    {
        public static List<TChange>? BruteForceFirst<TState, TChange>(
            TState initialState,
            TState finalState,
            Func<TState, List<TChange>, TState, IEnumerable<TChange>> getStateChanges,
            Func<TState, TChange, TState> applyStateChange,
            IEqualityComparer<TState>? stateEqualityComparer = null)
        {
            var equalityComparer = stateEqualityComparer ?? EqualityComparer<TState>.Default;

            var visited = new HashSet<TState>(equalityComparer);
            var queue = new Queue<(TState State, List<TChange> Changes)>();

            queue.Enqueue((initialState, new List<TChange>()));

            while (queue.TryDequeue(out var element))
            {
                var (currentState, currentChanges) = element;

                if (!visited.Add(currentState))
                {
                    continue;
                }

                if (equalityComparer.Equals(currentState, finalState))
                {
                    return currentChanges;
                }

                foreach (var change in getStateChanges(currentState, currentChanges, finalState))
                {
                    var nextState = applyStateChange(currentState, change);
                    var nextChanges = new List<TChange>(currentChanges) { change };
                    queue.Enqueue((nextState, nextChanges));
                }
            }

            return null;
        }
        
        public static List<TChange>? BruteForceBest<TState, TChange>(
            TState initialState,
            TState finalState,
            Func<TState, List<TChange>, TState, IEnumerable<TChange>> getStateChanges,
            Func<TState, TChange, TState> applyStateChange,
            Func<List<TChange>, double> getChangeCost,
            IEqualityComparer<TState>? stateEqualityComparer = null)
        {
            var equalityComparer = stateEqualityComparer ?? EqualityComparer<TState>.Default;

            var visited = new HashSet<TState>(equalityComparer);
            var queue = new Queue<(TState State, List<TChange> Changes)>();

            queue.Enqueue((initialState, new List<TChange>()));

            double bestCost = double.MaxValue;
            List<TChange>? bestResult = null; 
            while (queue.TryDequeue(out var element))
            {
                var (currentState, currentChanges) = element;

                var currentCost = getChangeCost(currentChanges);
                if (currentCost >= bestCost)
                {
                    continue;
                }
                
                if (equalityComparer.Equals(currentState, finalState))
                {
                    bestResult = currentChanges;
                    bestCost = currentCost;
                    continue;
                }
                
                if (!visited.Add(currentState))
                {
                    continue;
                }

                foreach (var change in getStateChanges(currentState, currentChanges, finalState))
                {
                    var nextState = applyStateChange(currentState, change);
                    var nextChanges = new List<TChange>(currentChanges) { change };
                    queue.Enqueue((nextState, nextChanges));
                }
            }

            return bestResult;
        }
        
        public static List<TChange>? GreedyBestFirst<TState, TChange>(
            TState initialState,
            TState finalState,
            Func<TState, List<TChange>, TState, IEnumerable<TChange>> getStateChanges,
            Func<TState, TChange, TState> applyStateChange,
            Func<TState, double> heuristic,
            IEqualityComparer<TState>? stateEqualityComparer = null)
        {
            var equalityComparer = stateEqualityComparer ?? EqualityComparer<TState>.Default;

            var visited = new HashSet<TState>(equalityComparer);
            var sortedList = new SortedList<(double HeuristicValue, ulong Counter), (TState State, List<TChange> Changes)>();

            ulong counter = 0;
            
            sortedList.Add((heuristic(initialState), counter++), (initialState, new List<TChange>()));

            while (sortedList.TryGetFirst(out var currentStateInfo))
            {
                sortedList.RemoveAt(0);
                var currentState = currentStateInfo.Value.State;
                var currentChanges = currentStateInfo.Value.Changes;

                if (!visited.Add(currentState))
                {
                    continue;
                }
                
                if (equalityComparer.Equals(currentState, finalState))
                {
                    return currentChanges;
                }


                foreach (var change in getStateChanges(currentState, currentChanges, finalState))
                {
                    var nextState = applyStateChange(currentState, change);
                    if (!visited.Contains(nextState))
                    {
                        var nextChanges = new List<TChange>(currentChanges) { change };
                        var nextHeuristicValue = heuristic(nextState);
                        sortedList.Add((nextHeuristicValue, counter++), (nextState, nextChanges));
                    }
                }
            }

            return null;
        }
    }
}