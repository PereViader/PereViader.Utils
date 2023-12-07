using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Extensions;
using PereViader.Utils.Common.TaskRunners;

namespace PereViader.Utils.Common.ApplicationContexts
{
    public class ApplicationContextService : IApplicationContextService
    {
        public event Action OnBeginApplicationContextChange;
        public event Action OnFinishApplicationContextChange;
        
        private readonly Stack<IApplicationContext> _contextStack = new Stack<IApplicationContext>();
        private readonly TaskRunner _taskRunner = new TaskRunner();
        
        public IApplicationContextChangeHandle Push(IApplicationContext applicationContext)
        {
            return Push(new [] { applicationContext });
        }

        public IApplicationContextChangeHandle Push(IEnumerable<IApplicationContext> applicationContexts)
        {
            var handle = new ApplicationContextChangeHandle();
            
            _taskRunner.RunSequencedAndForget((ct) => DoPush(applicationContexts, handle, ct));

            return handle;
        }

        public IApplicationContextChangeHandle Pop()
        {
            var predicate = DelegateExtensions.With<IApplicationContext>.TrueAfterNCallsPredicate(1);
            return PopUntil(predicate);
        }

        public IApplicationContextChangeHandle PopUntil<T>() where T : IApplicationContext
        {
            return PopUntil(x => x is T);
        }

        public IApplicationContextChangeHandle PopThenPush(IApplicationContext applicationContext)
        {
            var predicate = DelegateExtensions.With<IApplicationContext>.TrueAfterNCallsPredicate(1);
            return PopUntilThenPush(applicationContext, predicate);
        }

        public IApplicationContextChangeHandle PopThenPush(IEnumerable<IApplicationContext> applicationContexts)
        {
            var predicate = DelegateExtensions.With<IApplicationContext>.TrueAfterNCallsPredicate(1);
            return PopUntilThenPush(applicationContexts, predicate);        
        }

        public IApplicationContextChangeHandle PopUntilThenPush(IApplicationContext applicationContext, Predicate<IApplicationContext> predicate)
        {
            return PopUntilThenPush(new[] { applicationContext }, predicate);
        }

        public IApplicationContextChangeHandle PopUntilThenPush(IEnumerable<IApplicationContext> applicationContexts, Predicate<IApplicationContext> predicate)
        {
            var handle = new ApplicationContextChangeHandle();
            
            _taskRunner.RunSequencedAndForget((ct) => DoPopUntilThenPush(applicationContexts, predicate, handle, ct));

            return handle;
        }

        public IApplicationContextChangeHandle PopUntilThenPush<T>(IApplicationContext applicationContext) where T : IApplicationContext
        {
            return PopUntilThenPush(applicationContext, x => x is T);
        }

        public IApplicationContextChangeHandle PopUntilThenPush<T>(IEnumerable<IApplicationContext> applicationContexts) where T : IApplicationContext
        {
            return PopUntilThenPush(applicationContexts, x => x is T);
        }

        public IApplicationContextChangeHandle PopUntil(Predicate<IApplicationContext> predicate)
        {
            var handle = new ApplicationContextChangeHandle();
            
            _taskRunner.RunSequencedAndForget((ct) => DoPopUntil(predicate, handle, ct));

            return handle;
        }

        private async Task DoPush(IEnumerable<IApplicationContext> applicationContexts, ApplicationContextChangeHandle handle, CancellationToken cancellationToken)
        {
            OnBeginApplicationContextChange?.Invoke();
            
            handle.UpdateStep(ApplicationContextChangeStep.ProcessingPrevious);
            
            if (_contextStack.TryPeek(out var formerTopContext))
            {
                await formerTopContext.Suspend(cancellationToken);
            }

            await DoSharedPush(applicationContexts, handle, cancellationToken);
            
            OnFinishApplicationContextChange?.Invoke();
        }
        
        private async Task DoPopUntil(Predicate<IApplicationContext> predicate, ApplicationContextChangeHandle handle, CancellationToken cancellationToken)
        {
            OnBeginApplicationContextChange?.Invoke();

            await DoSharedPop(predicate, handle, cancellationToken);

            handle.UpdateStep(ApplicationContextChangeStep.AwaitingPermissionForFinal);
            await handle.WaitCompleteAllowed(cancellationToken);
            
            if (_contextStack.TryPeek(out var contextToResume))
            {
                handle.UpdateStep(ApplicationContextChangeStep.StartingFinal);
                await contextToResume.Resume(cancellationToken);
            }
            
            handle.UpdateStep(ApplicationContextChangeStep.Complete);
            OnFinishApplicationContextChange?.Invoke();
        }
        
        private async Task DoPopUntilThenPush(
            IEnumerable<IApplicationContext> applicationContexts,
            Predicate<IApplicationContext> predicate,
            ApplicationContextChangeHandle handle,
            CancellationToken cancellationToken)
        {
            OnBeginApplicationContextChange?.Invoke();
            await DoSharedPop(predicate, handle, cancellationToken);
            await DoSharedPush(applicationContexts, handle, cancellationToken);
            OnFinishApplicationContextChange?.Invoke();
        }

        private async Task DoSharedPop(Predicate<IApplicationContext> predicate, ApplicationContextChangeHandle handle,
            CancellationToken cancellationToken)
        {
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
            
            if (!found && !predicate(null))
            {
                throw new InvalidOperationException("Could not find desired IApplicationContext while popping");
            }
        }
        
        private async Task DoSharedPush(IEnumerable<IApplicationContext> applicationContexts, ApplicationContextChangeHandle handle,
            CancellationToken cancellationToken)
        {
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
        }
    }
}