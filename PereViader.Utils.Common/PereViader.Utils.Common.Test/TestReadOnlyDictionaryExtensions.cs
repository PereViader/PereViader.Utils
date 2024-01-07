using NUnit.Framework;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.Test;

public class TestReadOnlyDictionaryExtensions
{
    [Test]
    public void GetDifferingEntries_OnEqual_IsEmpty()
    {
        var dict1 = new Dictionary<int, int>()
        {
            { 1, 1 },
            { 2, 2 },
            { 3, 3 }
        };

        var dict2 = new Dictionary<int, int>(dict1);

        var differingEntries = dict1.GetDifferingEntries(dict2);
        
        Assert.That(differingEntries, Is.Empty);
    }
    
    [Test]
    public void GetDifferingEntries_Different_ProducesDifference()
    {
        var dict1 = new Dictionary<int, int>()
        {
            { 1, 1 },
            { 2, 2 },
            { 3, 3 }
        };

        var dict2 = new Dictionary<int, int>()
        {
            {2, 2},
            {3, 4}
        };

        var differingEntries = dict1.GetDifferingEntries(dict2);
        
        Assert.That(differingEntries, Is.EquivalentTo(new [] {new KeyValuePair<int, int>(1, 1), new KeyValuePair<int, int>(3, 3)}));
    }
}