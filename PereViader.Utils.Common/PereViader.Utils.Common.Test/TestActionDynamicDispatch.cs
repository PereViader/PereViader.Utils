using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using PereViader.Utils.Common.DynamicDispatch;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Common.Test
{
    [TestFixture]
    public class TestActionDynamicDispatch
    {
        public interface IInterface
        {
        }

        public class ImplA : IInterface
        {
        }

        public class ImplB : IInterface
        {
        }

        [Test]
        public void TryExecute_OnRegisteredType_RunsProperly()
        {
            var actionDynamicDispatch = new ActionDynamicDispatch<IInterface>();

            bool hasRunA = false;
            bool hasRunB = false;
            actionDynamicDispatch.Register<ImplA>(x => hasRunA = true);
            actionDynamicDispatch.Register<ImplB>(x => hasRunB = true);
            
            actionDynamicDispatch.Execute(new ImplA());
            
            Assert.That(hasRunA, Is.True);
            Assert.That(hasRunB, Is.False);
        }
        
        [Test]
        public void TryExecute_OnUnregisteredType_Fails()
        {
            var actionDynamicDispatch = new ActionDynamicDispatch<IInterface>();

            var couldRun = actionDynamicDispatch.TryExecute(new ImplA());
            
            Assert.That(couldRun, Is.False);
        }
    }

    [TestFixture]
    public class TestTaskExtensions
    {
        [Test]
        public void CreateLinkedTask_ThenCancelOriginalTask_CancelsCreatedTask()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var taskCompletionSource = new TaskCompletionSource<object>();

                var linkedTask = taskCompletionSource.Task.CreateLinkedTask(cancellationTokenSource.Token);

                taskCompletionSource.TrySetCanceled();
                
                Assert.That(linkedTask.IsCanceled);
            }
        }
        
        [Test]
        public void CreateLinkedTask_ThenCancelToken_CancelsCreatedTask()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var taskCompletionSource = new TaskCompletionSource<object>();

                var linkedTask = taskCompletionSource.Task.CreateLinkedTask(cancellationTokenSource.Token);

                cancellationTokenSource.Cancel();
                
                Assert.That(linkedTask.IsCanceled);
            }
        }
        
        [Test]
        public void CreateLinkedTask_ThenCompleteOriginal_CompletesCreatedTask()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var taskCompletionSource = new TaskCompletionSource<object>();

                var linkedTask = taskCompletionSource.Task.CreateLinkedTask(cancellationTokenSource.Token);

                var result = new object();
                taskCompletionSource.TrySetResult(result);
                
                Assert.That(linkedTask.IsCompleted);
                Assert.That(linkedTask.Result, Is.EqualTo(result));
            }
        }
    }
}