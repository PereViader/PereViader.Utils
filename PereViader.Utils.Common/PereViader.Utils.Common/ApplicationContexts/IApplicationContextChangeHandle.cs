using System;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Generators;

namespace PereViader.Utils.Common.ApplicationContexts
{
    /// <summary>
    /// This handle represents a change in the application context. The change will wait until AllowComplete is called on it
    /// before the IApplicationContext.Enter/Resume is called. This is done in order to allow external systems like loading screens
    /// to intervene in the loading process and prevent the context change to actually start until they are done
    /// </summary>
    [GenerateEventTaskWaits]
    [GenerateEventRefs]
    public interface IApplicationContextChangeHandle
    {
        event Action<ApplicationContextChangeStep> OnCurrentApplicationContextChangeStepUpdated;
        
        ApplicationContextChangeStep CurrentApplicationContextChangeStep { get; }

        bool IsCompleteAllowed { get; }

        Task WaitCompleteAllowed(CancellationToken ct);
        Task WaitComplete(CancellationToken ct);

        void AllowComplete();
    }
}