using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PereViader.Utils.Common.Extensions;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public class ApplicationContextService : IApplicationContextService
    {
        private readonly List<IApplicationContext> _applicationContexts = new();
        private readonly TaskRunner _taskRunner = new();

        public IReadOnlyList<IApplicationContext> ApplicationContexts => _applicationContexts;

        public IApplicationContextHandle Add(IApplicationContext applicationContext)
        {
            _applicationContexts.Add(applicationContext);

            return new ApplicationContextHandle(
                () => LoadContext(applicationContext),
                () => StartContext(applicationContext),
                () => UnloadContext(applicationContext)
            );
        }

        private Task LoadContext(IApplicationContext applicationContext)
        {
            return _taskRunner.RunSequenced((o, _) => o.Load(), applicationContext);
        }
        
        private Task StartContext(IApplicationContext applicationContext)
        {
            return _taskRunner.RunSequenced((o, _) => o.Start(), applicationContext);
        }

        private Task UnloadContext(IApplicationContext applicationContext)
        {
            return _taskRunner.RunSequenced((o, _) =>
            {
                o._applicationContexts.Remove(o.applicationContext);
                return o.applicationContext.DisposeAsync().AsTask();
            }, (applicationContext, _applicationContexts));
        }

        public T? Get<T>(Func<T, bool>? match = null) where T : IApplicationContext
        {
            var actualMatch = match ?? DelegateExtensions.With<T>.TrueFunc;
            return _applicationContexts.OfType<T>().FirstOrDefault(actualMatch);
        }
    }
}