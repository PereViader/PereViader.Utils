using NUnit.Framework;
using PereViader.Utils.Common.Generators;
using PereViader.Utils.Common.TestGenerateEventReferenceClassNamespace;

namespace PereViader.Utils.Common.TestGenerateEventReferenceClassNamespace
{
    public delegate void TestGenerateEventReferenceDelegate2(float banana, string apple);
}

namespace PereViader.Utils.Common.Test
{
    public delegate void TestGenerateEventReferenceDelegate0();
    public delegate void TestGenerateEventReferenceDelegate1(int potato);
    
    [GenerateEventRefs]
    public class TestGenerateEventRefsClass
    {
        public event TestGenerateEventReferenceDelegate0? Delegate0;
        public event TestGenerateEventReferenceDelegate1? Delegate1;
        public event TestGenerateEventReferenceDelegate2? Delegate2;
        
        public void InvokeDelegate0() => Delegate0?.Invoke();
        public void InvokeDelegate1(int potato) => Delegate1?.Invoke(potato);
        public void InvokeDelegate2(float banana, string apple) => Delegate2?.Invoke(banana, apple);
    }

    [TestFixture]
    public class TestGenerateEventRefs
    {
        [Test]
        public void TestDelegate0()
        {
            bool called = false;
            
            var obj = new TestGenerateEventRefsClass();
            var eventRef = obj.GetDelegate0EventRef();
            eventRef.Subscribe(() => called = true);
            
            obj.InvokeDelegate0();
            
            Assert.That(called, Is.True);
        }
        
        [Test]
        public void TestDelegate1()
        {
            int called = default;
            
            var obj = new TestGenerateEventRefsClass();
            var eventRef = obj.GetDelegate1EventRef();
            eventRef.Subscribe(x => called = x);
            
            obj.InvokeDelegate1(1);
            
            Assert.That(called, Is.EqualTo(1));
        }
        
        [Test]
        public void TestDelegate2()
        {
            (float, string) called = default;
            
            var obj = new TestGenerateEventRefsClass();
            var eventRef = obj.GetDelegate2EventRef();
            eventRef.Subscribe((x, y) => called = (x,y));
            
            obj.InvokeDelegate2(3f, "A");
            
            Assert.That(called, Is.EqualTo((3f, "A")));
        }
    }
}