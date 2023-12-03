using NUnit.Framework;
using PereViader.Utils.Common.Extensions;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.Test;

[TestFixture]
public class TestInstantTaskRunner
{
    [Test]
    public void Run_OnEmptyRunnerWithADummyTask_RunsInstantlyAndReturnsSameTask()
    {
        var taskCompletionSource = new TaskCompletionSource();
        using var runner = new InstantTaskRunner();
        
        var task = runner.Run((token, tcs) => tcs.Task, taskCompletionSource);
        
        Assert.That(task, Is.EqualTo(taskCompletionSource.Task));
    }
    
    [Test]
    public void Run_OnObjectThatWasPreviouslyCanceled_RunsInstantlyAndReturnsSameTask()
    {
        var taskCompletionSource = new TaskCompletionSource();
        using var runner = new InstantTaskRunner();
        runner.Cancel();
        
        var task = runner.Run((token, tcs) => tcs.Task, taskCompletionSource);
        
        Assert.That(task, Is.EqualTo(taskCompletionSource.Task));
    }

    [Test]
    public void Run_OnDisposed_ThrowsObjectDisposedException()
    {
        using var runner = new InstantTaskRunner();
        
        runner.Dispose();
        
        Assert.Throws<ObjectDisposedException>(() =>
        {
            _ = runner.Run(token => Task.CompletedTask);
        });
    }

    [Test]
    public void RunThenCancel_CancelsTheRun()
    {
        using var runner = new InstantTaskRunner();

        var task = runner.Run(token => token.CreateLinkedTask());
        runner.Cancel();

        Assert.IsTrue(task.IsCanceled);
    }

    [Test]
    public void Cancel_OnDisposed_DoesNotThrow()
    {
        using var runner = new InstantTaskRunner();
        runner.Dispose();
        Assert.DoesNotThrow(() => runner.Cancel());
    }
}