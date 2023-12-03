using NUnit.Framework;
using PereViader.Utils.Common.Extensions;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.Test;

[TestFixture]
public class TestSequencedTaskRunner
{
    [Test]
    public void Run_OneTaskThenAnother_RunsFirstTaskThenTheOther()
    {
        TaskCompletionSource<object?> tcs1 = null!;
        TaskCompletionSource<object?> tcs2 = null!;
        
        using var runner = new SequencedTaskRunner();
        var task1 = runner.RunWithCompletion(ct => (tcs1 = ct.CreateLinkedTaskCompletionSource<object?>()).Task);
        var task2 = runner.RunWithCompletion(ct => (tcs2 = ct.CreateLinkedTaskCompletionSource<object?>()).Task);

        Assert.That(task1.IsCompleted, Is.False);

        tcs1.TrySetResult(null);
        
        Assert.That(task1.IsCompleted, Is.True);
        Assert.That(task2.IsCompleted, Is.False);

        tcs2.TrySetResult(null);
        
        Assert.That(task2.IsCompleted, Is.True);
    }
    
    [Test]
    public void Run_AfterCanceling_RunsFine()
    {
        TaskCompletionSource<object?> tcs1 = null!;
        
        using var runner = new SequencedTaskRunner();
        
        runner.CancelRunning();
        var task1 = runner.RunWithCompletion(ct => (tcs1 = ct.CreateLinkedTaskCompletionSource<object?>()).Task);

        Assert.That(task1.IsCompleted, Is.False);

        tcs1.TrySetResult(null);
        
        Assert.That(task1.IsCompleted, Is.True);
    }
    
    [Test]
    public void RunThenCancel_BeforeAnyComplete_CancelsAllTasks()
    {
        using var runner = new SequencedTaskRunner();
        var task1 = runner.RunWithCompletion(ct => ct.CreateLinkedTask());
        var task2 = runner.RunWithCompletion(ct => ct.CreateLinkedTask());

        runner.CancelRunning();

        Assert.That(task1.IsCanceled, Is.True);
        Assert.That(task2.IsCanceled, Is.True);
    }

    [Test]
    public void Run_OnDisposedRunner_Throws()
    {
        var runner = new SequencedTaskRunner();

        runner.Dispose();
        
        Assert.Throws<ObjectDisposedException>(() => runner.RunWithoutCompletion(ct => ct.CreateLinkedTask()));
    }
}