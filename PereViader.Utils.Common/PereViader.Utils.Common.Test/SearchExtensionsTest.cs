using NUnit.Framework;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.Test;

public class SearchExtensionsTest
{
    [Test]
    public void BruteForceFirst()
    {
        int initialState = 0;
        int finalState = 6;
        var result = SearchExtensions.BruteForceFirst<int, int>(
            initialState,
            finalState,
            (state, c, finalState) => new [] { -4, 1, 10, 5 },
            (s, c) => s + c);

        Assert.That(result, Is.EquivalentTo(new [] { -4, 10 }));
    }
    
    [Test]
    public void BruteForceBest()
    {
        int initialState = 0;
        int finalState = 6;
        var result = SearchExtensions.BruteForceBest<int, int>(
            initialState,
            finalState,
            (state, c, finalState) => new [] { -4, 1, 10, 5 },
            (s, c) => s + c,
            c => c.Count * 1000 + c.Sum(Math.Abs));

        Assert.That(result, Is.EquivalentTo(new [] { 1, 5 }));
    }
    
    [Test]
    public void GreedyBestFirst()
    {
        // Let's allow jumping to any number between state-5 and state+5
        IEnumerable<int> GetPossibleChanges(int state, List<int> changes, int finalState) => new[] { -5, 0, 5, 1 };
        int ApplyChange(int state, int change) => state + change;
        double Heuristic(int state) => Math.Abs(state - 6); // Assuming finalState is 6 for simplicity
        
        int initialState = 0;
        int finalState = 6;
        var result = SearchExtensions.GreedyBestFirst<int, int>(
            initialState,
            finalState,
            GetPossibleChanges,
            ApplyChange,
            Heuristic);

        // We expect the path to jump to 1 and then jump to 6 by adding 5
        Assert.That(result, Is.EquivalentTo(new [] { 1, 5 }));
    }
}