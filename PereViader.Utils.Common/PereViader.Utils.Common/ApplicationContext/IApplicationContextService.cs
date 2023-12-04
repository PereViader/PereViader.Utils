using System;
using System.Collections.Generic;
using PereViader.Utils.Common.Generators;

namespace PereViader.Utils.Common.ApplicationContext
{
    [GenerateEventTaskWaits]
    public interface IApplicationContextService
    {
        event Action OnBeginApplicationContextChange;
        event Action OnFinishApplicationContextChange;

        IApplicationContextChangeHandle Push(IApplicationContext applicationContext);
        IApplicationContextChangeHandle Push(IEnumerable<IApplicationContext> applicationContexts);
        IApplicationContextChangeHandle Pop();
        IApplicationContextChangeHandle PopUntil(Predicate<IApplicationContext> predicate);
        IApplicationContextChangeHandle PopUntil<T>() where T : IApplicationContext;
        
        IApplicationContextChangeHandle PopThenPush(IApplicationContext applicationContext);
        IApplicationContextChangeHandle PopThenPush(IEnumerable<IApplicationContext> applicationContexts);
        IApplicationContextChangeHandle PopUntilThenPush(IApplicationContext applicationContext, Predicate<IApplicationContext> predicate);
        IApplicationContextChangeHandle PopUntilThenPush(IEnumerable<IApplicationContext> applicationContexts, Predicate<IApplicationContext> predicate);
        IApplicationContextChangeHandle PopUntilThenPush<T>(IApplicationContext applicationContext) where T : IApplicationContext;
        IApplicationContextChangeHandle PopUntilThenPush<T>(IEnumerable<IApplicationContext> applicationContexts) where T : IApplicationContext;
    }
}