using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public class ApplicationContextHandle : IApplicationContextHandle
    {
        public IApplicationContext ApplicationContext { get; }
        
        private readonly ApplicationContextService _applicationContextService;
        
        public ApplicationContextHandleStatus CurrentApplicationContextHandleStatus { get; private set; } =
            ApplicationContextHandleStatus.Awaiting;


        public ApplicationContextHandle(ApplicationContextService applicationContextService, IApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
            _applicationContextService = applicationContextService;
        }

        public async Task Load()
        {
            if (CurrentApplicationContextHandleStatus != ApplicationContextHandleStatus.Awaiting)
            {
                throw new InvalidOperationException(
                    $"Can't load unless the context is awaiting. It currently is [{CurrentApplicationContextHandleStatus}]");
            }

            await _applicationContextService.LoadContext(ApplicationContext);
            CurrentApplicationContextHandleStatus = ApplicationContextHandleStatus.Loaded;
        }

        public async Task Start()
        {
            if (CurrentApplicationContextHandleStatus != ApplicationContextHandleStatus.Loaded)
            {
                throw new InvalidOperationException(
                    $"Can't start unless the context is loaded. It currently is [{CurrentApplicationContextHandleStatus}]");
            }

            await _applicationContextService.StartContext(ApplicationContext);
            CurrentApplicationContextHandleStatus = ApplicationContextHandleStatus.Started;
        }

        public async ValueTask DisposeAsync()
        {
            if (CurrentApplicationContextHandleStatus is ApplicationContextHandleStatus.Disposing
                or ApplicationContextHandleStatus.Disposed)
            {
                return;
            }
            
            CurrentApplicationContextHandleStatus = ApplicationContextHandleStatus.Disposing;
            await _applicationContextService.DisposeContext(ApplicationContext);
            CurrentApplicationContextHandleStatus = ApplicationContextHandleStatus.Disposed;
        }
    }
}