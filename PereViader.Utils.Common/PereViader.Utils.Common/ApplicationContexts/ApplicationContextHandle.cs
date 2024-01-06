using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public class ApplicationContextHandle : IApplicationContextHandle
    {
        public event Action<ApplicationContextHandleStatus>? OnCurrentApplicationContextHandleStatusUpdated;

        private readonly Func<Task> _loadFunc;
        private readonly Func<Task> _startFunc;
        private readonly Func<Task> _unloadFunc;
        
        public ApplicationContextHandleStatus CurrentApplicationContextHandleStatus { get; private set; } =
            ApplicationContextHandleStatus.Awaiting;

        public ApplicationContextHandle(
            Func<Task> loadFunc, 
            Func<Task> startFunc, 
            Func<Task> unloadFunc)
        {
            _loadFunc = loadFunc;
            _startFunc = startFunc;
            _unloadFunc = unloadFunc;
        }

        public async Task Load()
        {
            if (CurrentApplicationContextHandleStatus != ApplicationContextHandleStatus.Awaiting)
            {
                throw new InvalidOperationException(
                    $"Can't load unless the context is awaiting. It currently is [{CurrentApplicationContextHandleStatus}]");
            }

            await _loadFunc.Invoke();
            CurrentApplicationContextHandleStatus = ApplicationContextHandleStatus.Loaded;
            OnCurrentApplicationContextHandleStatusUpdated?.Invoke(CurrentApplicationContextHandleStatus);
        }

        public async Task Start()
        {
            if (CurrentApplicationContextHandleStatus != ApplicationContextHandleStatus.Loaded)
            {
                throw new InvalidOperationException(
                    $"Can't start unless the context is loaded. It currently is [{CurrentApplicationContextHandleStatus}]");
            }

            await _startFunc.Invoke();
            CurrentApplicationContextHandleStatus = ApplicationContextHandleStatus.Started;
            OnCurrentApplicationContextHandleStatusUpdated?.Invoke(CurrentApplicationContextHandleStatus);
        }

        public async Task Unload()
        {
            await _unloadFunc.Invoke();
            CurrentApplicationContextHandleStatus = ApplicationContextHandleStatus.Unloaded;
            OnCurrentApplicationContextHandleStatusUpdated?.Invoke(CurrentApplicationContextHandleStatus);
        }
    }
}