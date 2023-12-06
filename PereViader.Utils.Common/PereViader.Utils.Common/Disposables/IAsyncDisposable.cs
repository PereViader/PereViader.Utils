using System.Threading.Tasks;

namespace PereViader.Utils.Common.Disposables
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