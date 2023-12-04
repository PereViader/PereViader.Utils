using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Extensions;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.ApplicationContext
{
    public class ApplicationContextService : IApplicationContextService
    {
        public event Action OnBeginApplicationContextChange;
        public event Action OnFinishApplicationContextChange;
        
        private readonly Stack<IApplicationContext> _contextStack = new Stack<IApplicationContext>();
        private readonly SequencedTaskRunner _sequencedTaskRunner = new SequencedTaskRunner();
        
        public IApplicationContextChangeHandle Push(IApplicationContext applicationContext)
        {
            return PushMany(new [] { applicationContext });
        }

        public IApplicationContextChangeHandle PushMany(IEnumerable<IApplicationContext> applicationContexts)
        {
            var handle = new ApplicationContextChangeHandle();
            
            _sequencedTaskRunner.RunAndForget((ct) => DoPushMany(applicationContexts, handle, ct));

            return handle;
        }

        private async Task DoPushMany(IEnumerable<IApplicationContext> applicationContexts, ApplicationContextChangeHandle handle, CancellationToken cancellationToken)
        {
            OnBeginApplicationContextChange?.Invoke();
            
            handle.UpdateStep(ApplicationContextChangeStep.ProcessingPrevious);
            
            if (_contextStack.TryPeek(out var formerTopContext))
            {
                await formerTopContext.Suspend(cancellationToken);
            }
            
            var readOnlyList = applicationContexts.ToReadOnlyList();
            for (var index = 0; index < readOnlyList.Count - 1; index++)
            {
                var applicationContext = readOnlyList[index];
                await applicationContext.Load(cancellationToken);
                await applicationContext.Suspend(cancellationToken);
                _contextStack.Push(applicationContext);
            }

            var last = readOnlyList[^1];

            _contextStack.Push(last);
            await last.Load(cancellationToken);
            
            handle.UpdateStep(ApplicationContextChangeStep.AwaitingPermissionForFinal);
            await handle.WaitCompleteAllowed(cancellationToken);
            
            handle.UpdateStep(ApplicationContextChangeStep.StartingFinal);
            await last.Enter(cancellationToken);
            
            handle.UpdateStep(ApplicationContextChangeStep.Complete);
            
            OnFinishApplicationContextChange?.Invoke();
        }
        
        public IApplicationContextChangeHandle Pop()
        {
            var firstDone = false;
            return PopUntil(_ =>
            {
                if (firstDone)
                {
                    return true;
                }
                firstDone = true;
                return false;
            });
        }

        public IApplicationContextChangeHandle PopUntil<T>() where T : IApplicationContext
        {
            return PopUntil(x => x is T);
        }
        
        public IApplicationContextChangeHandle PopUntil(Predicate<IApplicationContext> predicate)
        {
            var handle = new ApplicationContextChangeHandle();
            
            _sequencedTaskRunner.RunAndForget((ct) => DoPopUntil(predicate, handle, ct));

            return handle;
        }

        private async Task DoPopUntil(Predicate<IApplicationContext> predicate, ApplicationContextChangeHandle handle, CancellationToken cancellationToken)
        {
            OnBeginApplicationContextChange?.Invoke();
            handle.UpdateStep(ApplicationContextChangeStep.ProcessingPrevious);

            var currentApplicationContext = _contextStack.Peek();
            var found = false;
            while (_contextStack.TryPeek(out var context))
            {
                if (predicate(context) && context != currentApplicationContext)
                {
                    found = true;
                    break;
                }

                _contextStack.Pop();
                await context.Exit(cancellationToken);
            }

            if (!found)
            {
                throw new InvalidOperationException("Could not find desired IApplicationContext while popping");
            }

            var contextToResume = _contextStack.Peek();
            
            handle.UpdateStep(ApplicationContextChangeStep.AwaitingPermissionForFinal);
            await handle.WaitCompleteAllowed(cancellationToken);
                    
            handle.UpdateStep(ApplicationContextChangeStep.StartingFinal);
            await contextToResume.Resume(cancellationToken);
                    
            handle.UpdateStep(ApplicationContextChangeStep.Complete);
            OnFinishApplicationContextChange?.Invoke();
        }
    }
}