using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.Extensions;
using Godot;

namespace PereViader.Utils.Godot;

public static class SceneTreeExtensions
{
    public static Task AwaitTime(this SceneTree sceneTree, double seconds, bool processAlways = true, bool processInPhysics = false, bool ignoreTimeScale = false, CancellationToken cancellationToken = default)
    {
        var timer = sceneTree
            .CreateTimer(seconds, processAlways, processInPhysics, ignoreTimeScale);

        var tcs = new TaskCompletionSource<object>();
        timer.Timeout += () => tcs.SetResult(default!);
        tcs.LinkCancellationToken(cancellationToken);
        return tcs.Task;
    }
}