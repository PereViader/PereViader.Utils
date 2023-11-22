using System.Threading.Tasks;

namespace PereViader.Utils.Common.Disposing
{
    public interface IAsyncDisposable
    {
        Task DisposeAsync();
    }
    
    public interface IAsyncDisposable<out T> : IAsyncDisposable
    {
        T Value { get; }
    }
}