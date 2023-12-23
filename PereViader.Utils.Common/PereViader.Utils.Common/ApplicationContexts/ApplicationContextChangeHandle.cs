using System;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public class ApplicationContextChangeHandle : IApplicationContextChangeHandle
    {
        public event Action<ApplicationContextChangeStep> OnCurrentApplicationContextChangeStepUpdated;
        
        private readonly TaskCompletionSource<bool> _completeAllowedTaskCompletionSource = new TaskCompletionSource<bool>();
        public Task CompleteTask { get; set; }

        public ApplicationContextChangeStep CurrentApplicationContextChangeStep { get; private set; } =
            ApplicationContextChangeStep.WaitingToStart;
        
        public bool IsCompleteAllowed => _completeAllowedTaskCompletionSource.Task.IsCompleted;

        public Task WaitCompleteAllowed(CancellationToken ct) 
            => _completeAllowedTaskCompletionSource.Task.CreateLinkedTask(ct);
        
        public Task WaitComplete(CancellationToken ct)
            => CompleteTask.CreateLinkedTask(ct);

        public void AllowComplete()
            => _completeAllowedTaskCompletionSource.TrySetResult(true);

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
                CurrentApplicationContextChangeStep = i;
                OnCurrentApplicationContextChangeStepUpdated?.Invoke(i);
            }
        }
    }
}