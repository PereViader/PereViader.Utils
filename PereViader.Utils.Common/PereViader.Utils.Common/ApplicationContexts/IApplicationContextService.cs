using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public interface IApplicationContextService
    {
        IReadOnlyList<IApplicationContext> ApplicationContexts { get; }
        
        IApplicationContextHandle Add(IApplicationContext applicationContext);
        T? Get<T>(Func<T, bool>? match = null) where T : IApplicationContext;
    }
}