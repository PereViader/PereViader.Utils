using NUnit.Framework;
using PereViader.Utils.Common.TimeSlicing;

namespace PereViader.Utils.Common.Test;

[TestFixture]
public class TimeSlicerTest
{
    [Test]
    public void SliceAsync_OnNew_DoesNotSlice()
    {
        var dateTime = new DateTime(2000, 1, 1, 1, 1, 1);
        var slicer = new TimeSlicer(TimeSpan.FromSeconds(1), () => dateTime);

        var willSlice = slicer.WillSlice();
        var task = slicer.SliceAsync();
        
        Assert.That(willSlice, Is.False);
        Assert.That(task.IsCompleted, Is.True);
    }
    
    [Test]
    public void SliceAsync_AfterTime_DoesSliceOnce()
    {
        var dateTime = new DateTime(2000, 1, 1, 1, 1, 1);
        var slicer = new TimeSlicer(TimeSpan.FromSeconds(1), () => dateTime);

        dateTime += TimeSpan.FromSeconds(2);
        var willSlice1 = slicer.WillSlice();
        var task1 = slicer.SliceAsync();
        var willSlice2 = slicer.WillSlice();
        var task2 = slicer.SliceAsync();

        Assert.That(willSlice1, Is.True);
        Assert.That(task1.IsCompleted, Is.False);
        Assert.That(willSlice2, Is.False);
        Assert.That(task2.IsCompleted, Is.True);
    }
    
    [Test]
    public void SliceAsync_AfterReset_DoesNotSlice()
    {
        var dateTime = new DateTime(2000, 1, 1, 1, 1, 1);
        var slicer = new TimeSlicer(TimeSpan.FromSeconds(1), () => dateTime);

        dateTime += TimeSpan.FromSeconds(2);
        slicer.Reset();
        var willSlice = slicer.WillSlice();
        var task = slicer.SliceAsync();

        Assert.That(willSlice, Is.False);
        Assert.That(task.IsCompleted, Is.True);
    }
}