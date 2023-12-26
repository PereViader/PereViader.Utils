using System.Threading.Tasks;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class UnityTaskExtensions
    {
        public static async void RunAsync(this Task task)
        {
            await task;
        }
    }
}