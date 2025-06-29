using System;
using System.Collections.Generic;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public interface IApplicationContextService
    {
        IReadOnlyList<IApplicationContextHandle> ApplicationContextHandles { get; }
        
        IApplicationContextHandle Add(IApplicationContext applicationContext);
        IApplicationContextHandle? Get<T>(Func<T, bool>? match = null) where T : IApplicationContext;
    }
}