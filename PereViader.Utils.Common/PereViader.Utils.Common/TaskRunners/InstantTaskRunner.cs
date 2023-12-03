using System;
using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.TaskRunners
{
    public sealed class InstantTaskRunner : IDisposable
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public bool CanRun { get; private set; } = true;

        public Task Run(Func<CancellationToken, Task> func)
        {
            if (!CanRun)
            {
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            return func(_cancellationTokenSource.Token);
        }

        public Task Run<TArg>(Func<CancellationToken, TArg, Task> func, TArg arg)
        {
            if (!CanRun)
            {
                throw new ObjectDisposedException("InstantTaskRunner", "Cannot run task on a disposed TaskRunner.");
            }

            return func(_cancellationTokenSource.Token, arg);
        }

        public void Cancel()
        {
            if (!CanRun)
            {
                return;
            }
            
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            Cancel();
            CanRun = false;
        }
    }
}