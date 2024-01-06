using NUnit.Framework;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.Test;

public class SearchExtensionsTest
{
    [Test]
    public void BruteForce_OnCountFromZeroToThree_ReturnsAddOneThreeTimes()
    {
        int initialState = 0;
        int finalState = 3;
        var result = SearchExtensions.BruteForce<int, int>(
            initialState,
            finalState,
            (s, c) => new [] { -1, 1 },
            (s, c) => s + c);

        Assert.That(result, Is.EquivalentTo(new [] { 1, 1, 1 }));
    }
    
    [Test]
    public void TestNumberNavigation()
    {
        // Let's allow jumping to any number between state-5 and state+5
        IEnumerable<int> GetPossibleChanges(int state)
        {
            for (var i = -5; i <= 5; i++)
            {
                if (i != 0) yield return i;
            }
        }
        
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