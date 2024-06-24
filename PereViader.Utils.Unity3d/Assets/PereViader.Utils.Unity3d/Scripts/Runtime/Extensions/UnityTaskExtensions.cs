using System;
using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class UnityTaskExtensions
    {
        public static async void RunAsync(this Task task)
        {
            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        public static async Task WaitUntil(this Func<bool> func, CancellationToken ct)
        {
            while (!func())
            {
                await Task.Yield();
                ct.ThrowIfCancellationRequested();
            }
        }
        
        public static async Task WaitWhile(this Func<bool> func, CancellationToken ct)
        {
            while (func())
            {
                await Task.Yield();
                ct.ThrowIfCancellationRequested();
            }
        }
        
        public static async Task WaitFrame(CancellationToken ct)
        {
            await Task.Yield();
            ct.ThrowIfCancellationRequested();
        }
        
        public static async Task WaitFrames(int frameCount, CancellationToken ct)
        {
            for (int i = 0; i < frameCount; i++)
            {
                await Task.Yield();
                ct.ThrowIfCancellationRequested();
            }
        }
    }
}