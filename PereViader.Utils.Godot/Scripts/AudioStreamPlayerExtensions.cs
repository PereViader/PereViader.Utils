using System.Threading;
using System.Threading.Tasks;
using Godot;
using PereViader.Utils.Common.Extensions;

namespace PereViader.Utils.Godot;

public static class AudioStreamPlayerExtensions
{
    public static Task AwaitFinished(this AudioStreamPlayer audioStreamPlayer, CancellationToken cancellationToken)
    {
        TaskCompletionSource<object> tcs = new();
        tcs.LinkCancellationToken(cancellationToken);

        void Finished()
        {
            audioStreamPlayer.Finished -= Finished;
            tcs.TrySetResult(default!);
        }

        audioStreamPlayer.Finished += Finished;
        return tcs.Task;
    }
}