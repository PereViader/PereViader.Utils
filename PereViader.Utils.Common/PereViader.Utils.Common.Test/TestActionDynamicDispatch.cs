using NUnit.Framework;
using PereViader.Utils.Common.DynamicDispatch;

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
}