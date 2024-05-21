using System;
using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Godot;

public sealed class Awaiter<T> : IDisposable
{
    private TaskCompletionSource<T>? _tcs;

    public void Receive(T value)
    {
        var currentTcs = _tcs;
        _tcs = null;
        currentTcs?.TrySetResult(value);
    }

    public void Cancel()
    {
        var currentTcs = _tcs;
        _tcs = null;
        currentTcs?.TrySetCanceled();
    }

    public Task<T> AwaitNext(CancellationToken cancellationToken = default)
    {
        _tcs ??= new TaskCompletionSource<T>();

        return _tcs.Task.WaitAsync(cancellationToken);
    }

    public void Dispose()
    {
        _tcs?.TrySetCanceled();
    }
}