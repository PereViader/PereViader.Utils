using System;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public class ApplicationContextChangeHandle : IApplicationContextChangeHandle
    {
        public event Action<ApplicationContextChangeStep> OnCurrentApplicationContextChangeStepUpdated;
        
        private TaskCompletionSource<bool> _enterAllowedTaskCompletionSource = new TaskCompletionSource<bool>();
        private TaskCompletionSource<bool> _completeTaskCompletionSource = new TaskCompletionSource<bool>();

        public ApplicationContextChangeStep CurrentApplicationContextChangeStep { get; private set; } =
            ApplicationContextChangeStep.WaitingToStart;
        
        public bool IsCompleteAllowed => _enterAllowedTaskCompletionSource.Task.IsCompleted;

        public Task WaitCompleteAllowed(CancellationToken ct) 
            => _enterAllowedTaskCompletionSource.Task.CreateLinkedTask(ct);
        
        public Task WaitComplete(CancellationToken ct)
            => _completeTaskCompletionSource.Task.CreateLinkedTask(ct);

        public void AllowComplete()
            => _enterAllowedTaskCompletionSource.TrySetResult(true);

        public void UpdateStep(ApplicationContextChangeStep applicationContextChangeStep)
        {
            if (applicationContextChangeStep <= CurrentApplicationContextChangeStep)
            {
                throw new ArgumentOutOfRangeException(nameof(applicationContextChangeStep),
                    applicationContextChangeStep,
                    $"Application context update must be subsequent to the current one {CurrentApplicationContextChangeStep}");
            }
            
            for (ApplicationContextChangeStep i = CurrentApplicationContextChangeStep + 1; i <= applicationContextChangeStep; i++)
            {
                if(i == ApplicationContextChangeStep.Complete)
                {
                    _completeTaskCompletionSource.TrySetResult(true);
                }
                
                CurrentApplicationContextChangeStep = i;
                OnCurrentApplicationContextChangeStepUpdated?.Invoke(i);
            }
        }
    }
}