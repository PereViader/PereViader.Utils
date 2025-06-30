using NSubstitute;
using NUnit.Framework;
using PereViader.Utils.Common.ApplicationContexts;

namespace PereViader.Utils.Common.Test;

[TestFixture]
public class TestApplicationContextService
{
    [Test]
    public void AddThenUnload_RemovesTheContextAfterUnload()
    {
        var context = CreateApplicationContextThatCompletesInstantly();
        var service = new ApplicationContextService();
        var handle = service.Add(context);
        
        handle.DisposeAsync();
        
        Assert.That(service.ApplicationContextHandles, Is.Empty);
    }
    
    [Test]
    public async Task AddAndUseHandle_OnSingleHandle_RunsInExpectedOrder()
    {
        var context = CreateApplicationContextThatCompletesInstantly();
        var service = new ApplicationContextService();
        var handle = service.Add(context);

        await handle.Load(CancellationToken.None);
        await handle.Start(CancellationToken.None);
        await handle.DisposeAsync();
        
        Received.InOrder(() =>
        {
            context.Load(Arg.Any<CancellationToken>());
            context.Start(Arg.Any<CancellationToken>());
            context.DisposeAsync();
        });
    }
    
    [Test]
    public void AddThenGet_WithNullPredicate_FindsTheProperContext()
    {
        var context = Substitute.For<IApplicationContext>();
        var service = new ApplicationContextService();
        var handle = service.Add(context);

        var foundContextHandle = service.Get<IApplicationContext>();
        Assert.That(foundContextHandle, Is.EqualTo(handle));
    }
    
    [Test]
    public void AddThenGet_WithPredicate_FindsTheProperContext()
    {
        var context = Substitute.For<IApplicationContext>();
        var service = new ApplicationContextService();
        var handle = service.Add(context);

        var foundContext = service.Get<IApplicationContext>(x => x == context);
        Assert.That(foundContext, Is.EqualTo(handle));
    }

    [Test]
    public async Task AddAndUseHandle_OnMultipleHandles_RunsInExpectedOrder()
    {
        var context1 = CreateApplicationContextThatCompletesInstantly();
        var context2 = CreateApplicationContextThatCompletesInstantly();
        var service = new ApplicationContextService();
        var handle1 = service.Add(context1);
        var handle2 = service.Add(context2);
        
        await handle1.Load(CancellationToken.None);
        await handle2.Load(CancellationToken.None);
        await handle2.Start(CancellationToken.None);
        await handle1.Start(CancellationToken.None);
        await handle2.DisposeAsync();
        await handle1.DisposeAsync();
        
        Received.InOrder(() =>
        {
            context1.Load(Arg.Any<CancellationToken>());
            context2.Load(Arg.Any<CancellationToken>());
            context2.Start(Arg.Any<CancellationToken>());
            context1.Start(Arg.Any<CancellationToken>());
            context2.DisposeAsync();
            context1.DisposeAsync();
        });
    }

    private static IApplicationContext CreateApplicationContextThatCompletesInstantly()
    {
        var context = Substitute.For<IApplicationContext>();

        context.Load(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        context.Start(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        context.DisposeAsync().Returns(new ValueTask());

        return context;
    }
}