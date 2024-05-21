using Godot;

namespace PereViader.Utils.Godot;

public static class TimerExtensions
{
    public static SignalAwaiter StartAndAwait(this Timer timer)
    {
        var signal = timer.ToSignal(timer, Timer.SignalName.Timeout);
        timer.Start();
        return signal;
    }
}