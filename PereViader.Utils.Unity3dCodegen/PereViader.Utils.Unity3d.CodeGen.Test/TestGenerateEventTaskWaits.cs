using NUnit.Framework;
using PereViader.Utils.Unity3d.CodeGen.Runtime;

public delegate void TestGenerateEventTaskWaitsDelegate0();
public delegate void TestGenerateEventTaskWaitsDelegate1(int potato);
public delegate void TestGenerateEventTaskWaitsDelegate3(float orange, string lemon);

[GenerateEventTaskWaits]
public class SomeClass
 {
     public event TestGenerateEventTaskWaitsDelegate0? OnDelegate0;
     public event TestGenerateEventTaskWaitsDelegate1? OnDelegate1;
     public event TestGenerateEventTaskWaitsDelegate3? OnDelegate2;

     public void InvokeOnDelegate0() => OnDelegate0?.Invoke();
     public void InvokeOnDelegate1(int potato) => OnDelegate1?.Invoke(potato);
     public void InvokeOnDelegate2(float orange, string lemon) => OnDelegate2?.Invoke(orange, lemon);
 }

[TestFixture]
public class TestGenerateEventTaskWaits
{
    [Test]
    public void TestActionDelegate0()
    {
        var testClass = new SomeClass();
        var task = testClass.WaitOnDelegate0();
        
        Assert.That(task.IsCompleted, Is.False);
        testClass.InvokeOnDelegate0();
        Assert.That(task.IsCompleted, Is.True);
    }
    
    [Test]
    public void TestActionDelegate1()
    {
        var testClass = new SomeClass();
        var task = testClass.WaitOnDelegate1();
    
        Assert.That(task.IsCompleted, Is.False);
        testClass.InvokeOnDelegate1(10);
        Assert.That(task.IsCompleted, Is.True);
        Assert.That(task.Result, Is.EqualTo(10));
    }
    
    [Test]
    public void TestActionDelegate2()
    {
        var testClass = new SomeClass();
        var task = testClass.WaitOnDelegate2();
        
        Assert.That(task.IsCompleted, Is.False);
        testClass.InvokeOnDelegate2(10f, "lemon");
        Assert.That(task.IsCompleted, Is.True);
        Assert.That(task.Result, Is.EqualTo((10f, "lemon")));
    }
    
    [Test]
    public void TestActionDelegate0Cancellation()
    {
        var testClass = new SomeClass();
        using var cts = new CancellationTokenSource();
        var task = testClass.WaitOnDelegate0(cts.Token);
    
        cts.Cancel();
        Assert.That(task.IsCanceled);
    }
    
    [Test]
    public void TestActionDelegate1Cancellation()
    {
        var testClass = new SomeClass();
        using var cts = new CancellationTokenSource();
        var task = testClass.WaitOnDelegate1(cts.Token);
    
        cts.Cancel();
        Assert.That(task.IsCanceled);
    }
    
    [Test]
    public void TestActionDelegate2Cancellation()
    {
        var testClass = new SomeClass();
        using var cts = new CancellationTokenSource();
        var task = testClass.WaitOnDelegate2(cts.Token);
    
        cts.Cancel();
        Assert.That(task.IsCanceled);
    }
}

