using NUnit.Framework;
using PereViader.Utils.Common.Extensions;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.Test;

[TestFixture]
public class TestTaskRunner
{
    [Test]
    public void RunInstantly_OnEmptyRunnerWithADummyTask_RunsInstantlyAndReturnsSameTask()
    {
        var taskCompletionSource = new TaskCompletionSource();
        using var runner = new TaskRunner();
        
        var task = runner.RunInstantly((tcs, token) => tcs.Task, taskCompletionSource);
        
        Assert.That(task, Is.EqualTo(taskCompletionSource.Task));
    }
    
    [Test]
    public void RunInstantly_OnObjectThatWasPreviouslyCanceled_RunsInstantlyAndReturnsSameTask()
    {
        var taskCompletionSource = new TaskCompletionSource();
        using var runner = new TaskRunner();
        runner.CancelRunning();
        
        var task = runner.RunInstantly((tcs, token) => tcs.Task, taskCompletionSource);
        
        Assert.That(task, Is.EqualTo(taskCompletionSource.Task));
    }

    [Test]
    public void RunInstantly_OnDisposed_ThrowsObjectDisposedException()
    {
        using var runner = new TaskRunner();
        
        runner.Dispose();
        
        Assert.Throws<ObjectDisposedException>(() =>
        {
            _ = runner.RunInstantly(token => Task.CompletedTask);
        });
    }

    [Test]
    public void RunInstantlyThenCancel_CancelsTheRun()
    {
        using var runner = new TaskRunner();

        var task = runner.RunInstantly(token => token.CreateLinkedTask());
        runner.CancelRunning();

        Assert.IsTrue(task.IsCanceled);
    }

    [Test]
    public void Cancel_OnDisposed_DoesNotThrow()
    {
        using var runner = new TaskRunner();
        runner.Dispose();
        Assert.DoesNotThrow(() => runner.CancelRunning());
    }
    
    [Test]
    public void RunSequenced_OneTaskThenAnother_RunsFirstTaskThenTheOther()
    {
        TaskCompletionSource<object?> tcs1 = null!;
        TaskCompletionSource<object?> tcs2 = null!;
        
        using var runner = new TaskRunner();
        var task1 = runner.RunSequenced(ct => (tcs1 = ct.CreateLinkedTaskCompletionSource<object?>()).Task);
        var task2 = runner.RunSequenced(ct => (tcs2 = ct.CreateLinkedTaskCompletionSource<object?>()).Task);

        Assert.That(task1.IsCompleted, Is.False);

        tcs1.TrySetResult(null);
        
        Assert.That(task1.IsCompleted, Is.True);
        Assert.That(task2.IsCompleted, Is.False);

        tcs2.TrySetResult(null);
        
        Assert.That(task2.IsCompleted, Is.True);
    }
    
    [Test]
    public void RunSequenced_AfterCanceling_RunsFine()
    {
        TaskCompletionSource<object?> tcs1 = null!;
        
        using var runner = new TaskRunner();
        
        runner.CancelRunning();
        var task1 = runner.RunSequenced(ct => (tcs1 = ct.CreateLinkedTaskCompletionSource<object?>()).Task);

        Assert.That(task1.IsCompleted, Is.False);

        tcs1.TrySetResult(null);
        
        Assert.That(task1.IsCompleted, Is.True);
    }
    
    [Test]
    public void RunSequencedThenCancel_BeforeAnyComplete_CancelsAllTasks()
    {
        using var runner = new TaskRunner();
        var task1 = runner.RunSequenced(ct => ct.CreateLinkedTask());
        var task2 = runner.RunSequenced(ct => ct.CreateLinkedTask());

        runner.CancelRunning();

        Assert.That(task1.IsCanceled, Is.True);
        Assert.That(task2.IsCanceled, Is.True);
    }

    [Test]
    public void RunSequenced_OnDisposedRunner_Throws()
    {
        var runner = new TaskRunner();

        runner.Dispose();

        var task = runner.RunSequenced(ct => ct.CreateLinkedTask());
        Assert.That(task.Exception?.InnerException, Is.InstanceOf<ObjectDisposedException>());
    }
}