using NSubstitute;
using NUnit.Framework;
using PereViader.Utils.Common.ApplicationContext;

namespace PereViader.Utils.Common.Test;

[TestFixture]
public class TestApplicationContextService
{
    [Test]
    public void PushMany_TwoElementsOnEmptyStack_RunsAsExpected()
    {
        var context1 = CreateApplicationContextThatCompletesInstantly();
        var context2 = CreateApplicationContextThatCompletesInstantly();

        var service = new ApplicationContextService();

        service
            .Push(new[] { context1, context2 })
            .AllowComplete();
        
        Received.InOrder(() =>
        {
            context1.Load(Arg.Any<CancellationToken>());
            context1.Suspend(Arg.Any<CancellationToken>());
            context2.Load(Arg.Any<CancellationToken>());
            context2.Enter(Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public void Push_OnContext_DoesNotCompleteUntilTheElementIsAllowed()
    {
        var context = CreateApplicationContextThatCompletesInstantly();

        var service = new ApplicationContextService();

        var handle = service.Push(context);

        Assert.That(handle.IsCompleteAllowed, Is.False);
        Assert.That(handle.CurrentApplicationContextChangeStep, Is.EqualTo(ApplicationContextChangeStep.AwaitingPermissionForFinal));
        
        handle.AllowComplete();
        
        Assert.That(handle.IsCompleteAllowed, Is.True);
        Assert.That(handle.CurrentApplicationContextChangeStep, Is.EqualTo(ApplicationContextChangeStep.Complete));
    }
    
    [Test]
    public void PushThenPop_OnStackWithSingleElement_AddsItThenRemovesIt()
    {
        var context = CreateApplicationContextThatCompletesInstantly();

        var service = new ApplicationContextService();

        service
            .Push(context)
            .AllowComplete();
        
        service
            .Pop()
            .AllowComplete();

        Received.InOrder(() =>
        {
            context.Load(Arg.Any<CancellationToken>());
            context.Enter(Arg.Any<CancellationToken>());
            context.Exit(Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public void PushThenPop_OnEmptyStack_AddsThemSuspendsAndResumes()
    {
        var context1 = CreateApplicationContextThatCompletesInstantly();
        var context2 = CreateApplicationContextThatCompletesInstantly();
        
        var service = new ApplicationContextService();

        service
            .Push(new []{ context1, context2})
            .AllowComplete();
        
        service
            .Pop()
            .AllowComplete();

        Received.InOrder(() =>
        {
            context1.Load(Arg.Any<CancellationToken>());
            context1.Suspend(Arg.Any<CancellationToken>());
            context2.Load(Arg.Any<CancellationToken>());
            context2.Enter(Arg.Any<CancellationToken>());
            context2.Exit(Arg.Any<CancellationToken>());
            context1.Resume(Arg.Any<CancellationToken>());
        });
    }
    
    [Test]
    public void PopAndPush_OnStackWithOneContext_RunsAsExpected()
    {
        var context1 = CreateApplicationContextThatCompletesInstantly();
        var context2 = CreateApplicationContextThatCompletesInstantly();
        var context3 = CreateApplicationContextThatCompletesInstantly();

        var service = new ApplicationContextService();

        service
            .Push(context1)
            .AllowComplete();
        
        service
            .PopThenPush(new []{ context2, context3 })
            .AllowComplete();

        Received.InOrder(() =>
        {
            context1.Load(Arg.Any<CancellationToken>());
            context1.Enter(Arg.Any<CancellationToken>());
            context1.Exit(Arg.Any<CancellationToken>());
            context2.Load(Arg.Any<CancellationToken>());
            context2.Suspend(Arg.Any<CancellationToken>());
            context3.Load(Arg.Any<CancellationToken>());
            context3.Enter(Arg.Any<CancellationToken>());
        });
    }

    private static IApplicationContext CreateApplicationContextThatCompletesInstantly()
    {
        var context = Substitute.For<IApplicationContext>();

        context.Load(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        context.Enter(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        context.Suspend(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        context.Resume(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        context.Exit(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        return context;
    }
}