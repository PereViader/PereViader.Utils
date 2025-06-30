using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Extensions;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public class ApplicationContextService : IApplicationContextService, IDisposable
    {
        private readonly List<IApplicationContextHandle> _applicationContextHandles = new();
        private readonly TaskRunner _taskRunner = new();

        public IReadOnlyList<IApplicationContextHandle> ApplicationContextHandles => _applicationContextHandles;

        public IApplicationContextHandle Add(IApplicationContext applicationContext)
        {
            var applicationContextHandle = new ApplicationContextHandle(
                this,
                applicationContext);
            
            _applicationContextHandles.Add(applicationContextHandle);

            return applicationContextHandle;
        }

        internal Task LoadContext(IApplicationContext applicationContext, CancellationToken ct)
        {
            return _taskRunner.RunSequenced((o, ct) => o.Load(ct), applicationContext, ct);
        }
        
        internal Task StartContext(IApplicationContext applicationContext, CancellationToken ct)
        {
            return _taskRunner.RunSequenced((o, ct) => o.Start(ct), applicationContext, ct);
        }

        internal Task DisposeContext(IApplicationContextHandle applicationContextHandle)
        {
            return _taskRunner.RunSequenced(static async (o, _) =>
            {
                o.@this._applicationContextHandles.Remove(o.applicationContextHandle);
                await o.applicationContextHandle.ApplicationContext.DisposeAsync();
            }, (applicationContextHandle, @this: this));
        }

        public IApplicationContextHandle? Get<T>(Func<T, bool>? match = null) where T : IApplicationContext
        {
            var actualMatch = match ?? DelegateExtensions.With<T>.TrueFunc;
            foreach (var applicationContextHandle in _applicationContextHandles)
            {
                if (applicationContextHandle.ApplicationContext is T context && actualMatch(context))
                {
                    return applicationContextHandle;
                }
            }
            return null;
        }

        public void Dispose()
        {
            _taskRunner.Dispose();
        }
    }
}