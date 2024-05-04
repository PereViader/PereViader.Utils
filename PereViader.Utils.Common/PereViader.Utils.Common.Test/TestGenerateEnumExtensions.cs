using NUnit.Framework;
using PereViader.Utils.Common.Generators;

namespace PereViader.Utils.Common.Test;

[GenerateEnumExtensions]
public enum SomeEnum
{
    Value1,
    Value2,
}

[TestFixture]
public class TestGenerateEnumExtensions
{
    [Test]
    public void TestValues()
    {
        Assert.That(SomeEnumExtensions.Values, Is.EquivalentTo(new []{SomeEnum.Value1, SomeEnum.Value2}));
    }
    
    [Test]
    public void TestToStringOptimized()
    {
        Assert.That(SomeEnum.Value1.ToStringOptimized(), Is.EqualTo(nameof(SomeEnum.Value1)));
    }
    
    [Test]
    public void TestParse()
    {
        Assert.That(SomeEnumExtensions.ParseOptimized(nameof(SomeEnum.Value1))!.Value, Is.EqualTo(SomeEnum.Value1));
    }
}