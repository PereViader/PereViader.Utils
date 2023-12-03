// using NUnit.Framework;
// using PereViader.Utils.Unity3d.CodeGen.Runtime;
//
// namespace PereViader.Utils.Unity3d.CodeGen.Test;
//
// public delegate void TestGenerateEventReferenceDelegate0();
// public delegate void TestGenerateEventReferenceDelegate1(int potato);
// public delegate void TestGenerateEventReferenceDelegate2(float banana, string apple);
//
// //[GenerateEventReferences]
// public class TestGenerateEventReferenceClass
// {
//     public event TestGenerateEventReferenceDelegate0? Delegate0;
//     public event TestGenerateEventReferenceDelegate1? Delegate1;
//     public event TestGenerateEventReferenceDelegate2? Delegate2;
//
//     public void InvokeDelegate0() => Delegate0?.Invoke();
//     public void InvokeDelegate1(int potato) => Delegate1?.Invoke(potato);
//     public void InvokeDelegate2(float banana, string apple) => Delegate2?.Invoke(banana, apple);
// }
//
// public readonly struct EventRef<TObject, TDelegate>
//     where TDelegate : Delegate
// {
//     public TObject ObjectReference { get; }
//     private readonly Action<TObject, TDelegate> subscribeAction;
//     private readonly Action<TObject, TDelegate> unsubscribeAction;
//     
//     public EventRef(TObject value, Action<TObject, TDelegate> subscribeAction, Action<TObject, TDelegate> unsubscribeAction)
//     {
//         ObjectReference = value;
//         this.subscribeAction = subscribeAction;
//         this.unsubscribeAction = unsubscribeAction;
//     }
//
//     public void Subscribe(TDelegate action)
//     {
//         subscribeAction.Invoke(ObjectReference, action);
//     }
//     
//     public void Unsubscribe(TDelegate action)
//     {
//         unsubscribeAction.Invoke(ObjectReference, action);
//     }
// }
//
// public static class TestGenerateEventReferenceClassEventRefEExtensions
// {
//     private static readonly Action<TestGenerateEventReferenceClass, TestGenerateEventReferenceDelegate0> Delegate0SubscribeAction = (obj, action) => obj.Delegate0 += action;
//     private static readonly Action<TestGenerateEventReferenceClass, TestGenerateEventReferenceDelegate0> Delegate0UnsubscribeAction = (obj, action) => obj.Delegate0 -= action;
//     
//     public static EventRef<TestGenerateEventReferenceClass, TestGenerateEventReferenceDelegate0> GetDelegate0EventRef(
//         this TestGenerateEventReferenceClass obj)
//     {
//         return new EventRef<TestGenerateEventReferenceClass, TestGenerateEventReferenceDelegate0>(
//             obj, 
//             Delegate0SubscribeAction,
//             Delegate0UnsubscribeAction);
//     }
// }
//
// [TestFixture]
// public class TestGenerateEventReferences
// {
//     [Test]
//     public void TestEventReferenceSubscribesProperly()
//     {
//         bool called = false;
//         
//         var obj = new TestGenerateEventReferenceClass();
//         var eventRef = obj.GetDelegate0EventRef();
//         eventRef.Subscribe(() => called = true);
//         
//         obj.InvokeDelegate0();
//         
//         Assert.That(called, Is.True);
//     }
// }