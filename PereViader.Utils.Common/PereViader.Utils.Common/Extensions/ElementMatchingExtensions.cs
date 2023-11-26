using System;
using System.Collections.Generic;
using System.Linq;

namespace PereViader.Utils.Common.Extensions
{
    public static class ElementMatchingExtensions
    {
        /// <summary>
        /// Attempts to find pairs of elements between two lists based on the provided criteria.
        /// </summary>
        /// <typeparam name="T">The type of elements in the lists.</typeparam>
        /// <param name="elements1">The first list of elements.</param>
        /// <param name="elements2">The second list of elements.</param>
        /// <param name="isValidPairCandidateFunc">A function that determines if two elements are a valid pair.</param>
        /// <param name="matchedPairs">The list of matched pairs, if successful.</param>
        /// <returns>
        /// <c>true</c> if valid pairs are found; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This function uses a depth-first search (DFS)-like approach to find valid pairs between the two lists.
        /// It explores possible pairs based on the provided criteria and backtracks when necessary to explore other possibilities.
        /// </remarks>
        public static bool TryMatchPairsDepthFirstSearch<T>(
            List<T> elements1,
            List<T> elements2,
            Func<T, T, bool> isValidPairCandidateFunc,
            out List<(T, T)> matchedPairs)
        {
            var minGroupSize = Math.Min(elements1.Count, elements2.Count);

            Stack<int> decisionStack = new Stack<int>();

            var nextCandidate = DepthFirstSearchMatchPairsTryGetNextValidPartner(
                elements1[0],
                0,
                elements2,
                decisionStack,
                isValidPairCandidateFunc);

            if (nextCandidate is null)
            {
                matchedPairs = default;
                return false;
            }

            decisionStack.Push(nextCandidate.Value);

            bool shouldPop = false;

            while (decisionStack.Count < minGroupSize)
            {
                var nextStartingChoice = 0;

                if (shouldPop)
                {
                    nextStartingChoice = decisionStack.Pop();
                    shouldPop = false;

                    if (decisionStack.Count == 0)
                    {
                        matchedPairs = default;
                        return false;
                    }
                }

                nextCandidate = DepthFirstSearchMatchPairsTryGetNextValidPartner(
                    elements1[decisionStack.Count],
                    nextStartingChoice,
                    elements2,
                    decisionStack,
                    isValidPairCandidateFunc);

                if (nextCandidate is null)
                {
                    shouldPop = true;
                    continue;
                }

                decisionStack.Push(nextCandidate.Value);
            }

            matchedPairs = decisionStack
                .Reverse()
                .Select((x, i) => (elements1[i], elements2[x]))
                .ToList();
            return true;
        }

        /// <summary>
        /// Tries to find the next valid partner for a given element in a list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="currentElement">The current element for which a partner is sought.</param>
        /// <param name="startIndex">The starting index in the list for the search.</param>
        /// <param name="elements">The list of elements to search for a partner.</param>
        /// <param name="decisionStack">A stack that keeps track of selected elements.</param>
        /// <param name="isValidPairCandidateFunc">A function that determines if two elements are a valid pair.</param>
        /// <returns>
        /// The index of the next valid partner element in the list, or <c>null</c> if no valid partner is found.
        /// </returns>
        private static int? DepthFirstSearchMatchPairsTryGetNextValidPartner<T>(
            T currentElement,
            int startIndex,
            List<T> elements,
            Stack<int> decisionStack,
            Func<T, T, bool> isValidPairCandidateFunc)
        {
            for (int i = startIndex; i < elements.Count; i++)
            {
                if (decisionStack.Contains(i))
                {
                    continue;
                }

                var candidateElement = elements[i];
                if (!isValidPairCandidateFunc(currentElement, candidateElement))
                {
                    continue;
                }

                return i;
            }

            return null;
        }
    }
}