using NUnit.Framework;
using PereViader.Utils.Common.Generators;

namespace PereViader.Utils.Common.Test;

[GenerateToString]
internal partial class GenerateToStringClass1
{
    public int A { get; }
    public string B { get; }
    public GenerateToStringClass2 SomeChild { get; }

    public GenerateToStringClass1(int a, string b, GenerateToStringClass2 someChild)
    {
        A = a;
        B = b;
        SomeChild = someChild;
    }
}

[GenerateToString]
public partial class GenerateToStringClass2
{
    public float C { get; }

    public GenerateToStringClass2(float c)
    {
        C = c;
    }
}

[TestFixture]
public class TestGenerateToString
{
    [Test]
    public void ToString_WorksAsExpected()
    {
        var someClass = new GenerateToStringClass1(1, "2", new GenerateToStringClass2(3.3f));
        Assert.That(someClass.ToString(), Is.EqualTo("[ A:1, B:2, SomeChild:[ C:3,3 ] ]"));
    }
}