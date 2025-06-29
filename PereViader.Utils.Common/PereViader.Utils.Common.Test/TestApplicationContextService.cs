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

        await handle.Load();
        await handle.Start();
        await handle.DisposeAsync();
        
        Received.InOrder(() =>
        {
            context.Load();
            context.Start();
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
        
        await handle1.Load();
        await handle2.Load();
        await handle2.Start();
        await handle1.Start();
        await handle2.DisposeAsync();
        await handle1.DisposeAsync();
        
        Received.InOrder(() =>
        {
            context1.Load();
            context2.Load();
            context2.Start();
            context1.Start();
            context2.DisposeAsync();
            context1.DisposeAsync();
        });
    }

    private static IApplicationContext CreateApplicationContextThatCompletesInstantly()
    {
        var context = Substitute.For<IApplicationContext>();

        context.Load().Returns(Task.CompletedTask);
        context.Start().Returns(Task.CompletedTask);
        context.DisposeAsync().Returns(new ValueTask());

        return context;
    }
}