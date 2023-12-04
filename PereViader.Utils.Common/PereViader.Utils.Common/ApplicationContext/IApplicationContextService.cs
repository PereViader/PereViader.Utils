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
        IApplicationContextChangeHandle PushMany(IEnumerable<IApplicationContext> applicationContexts);
        IApplicationContextChangeHandle Pop();
        IApplicationContextChangeHandle PopUntil(Predicate<IApplicationContext> predicate);
        IApplicationContextChangeHandle PopUntil<T>() where T : IApplicationContext;
    }
}