using NUnit.Framework;
using PereViader.Utils.Common.ActiveStatus;

namespace PereViader.Utils.Common.Test;

[TestFixture]
public class TestActiveStatus
{
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void IsActive_OnElementWithNoCallsAndDefaultTrue_IsTheDefault(bool defaultState)
    {
        var activeStatus = new ActiveStatus<int>(defaultState);
            
        Assert.That(activeStatus.IsActive(0), Is.EqualTo(defaultState));
    }
        
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void IsActive_OnElementThatWasRegisteredAsTheContrary_IsTheContrary(bool defaultState)
    {
        var activeStatus = new ActiveStatus<int>(defaultState);

        activeStatus.UpdateStatus(new object(), 0, !defaultState);

        Assert.That(activeStatus.IsActive(0), Is.EqualTo(!defaultState));
    }
        
    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void IsActive_AfterForgetting_IsResetToDefault(bool defaultState)
    {
        var activeStatus = new ActiveStatus<int>(defaultState);

        activeStatus.UpdateStatus(new object(), 0, !defaultState);
        activeStatus.Forget(0);

        Assert.That(activeStatus.IsActive(0), Is.EqualTo(defaultState));
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void UpdateStatus_ProperlyUpdatesStatusEachTime(bool defaultState)
    {
        var activeStatus = new ActiveStatus<int>(defaultState);

        var update1 = activeStatus.UpdateStatus(new object(), 0, defaultState);
        var update2 = activeStatus.UpdateStatus(new object(), 0, !defaultState);
        var update3 = activeStatus.UpdateStatus(new object(), 0, !defaultState);
        var update4 = activeStatus.UpdateStatus(new object(), 0, defaultState);
        var isActive = activeStatus.IsActive(0);

        Assert.That(update1, Is.EqualTo(false));
        Assert.That(update2, Is.EqualTo(true));
        Assert.That(update3, Is.EqualTo(false));
        Assert.That(update4, Is.EqualTo(false));
        Assert.That(isActive, Is.EqualTo(!defaultState));
    }
}