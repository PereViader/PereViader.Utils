using System.Threading.Tasks;

namespace PereViader.Utils.Godot;

public static class TaskExtensions
{
    public static async void RunAsync(this Task task)
    {
        try
        {
            await task;
        }
        catch (TaskCanceledException)
        {
        }
    }
}