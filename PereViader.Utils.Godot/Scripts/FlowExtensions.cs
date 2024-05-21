using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Godot;

public static class FlowExtensions
{
    public delegate bool FirstTimeActionDelegate(bool isFirstTime);

    public static void RepeatUntilSuccess(FirstTimeActionDelegate action)
    {
        var succeeded = action.Invoke(true);
        while (!succeeded)
        {
            succeeded = action.Invoke(false);
        }
    }
    
    public delegate Task<bool> FirstTimeActionDelegateAsync(bool isFirstTime, CancellationToken cancellationToken);

    public static async Task RepeatUntilSuccess(FirstTimeActionDelegateAsync action, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var succeeded = await action.Invoke(true, cancellationToken);
        while (!succeeded)
        {
            cancellationToken.ThrowIfCancellationRequested();
            succeeded = await action.Invoke(false, cancellationToken);
        }
    }
}