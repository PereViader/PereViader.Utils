using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.ApplicationContexts
{
    /// <summary>
    /// An application context is an area of your application that has its own separate identity from the rest of the app.
    /// The IApplicationContext API just knows how to load, start and dispose of the context.
    /// The context should not be used directly. It should be used from <see cref="IApplicationContextHandle"/> instead.
    /// </summary>
    public interface IApplicationContext : IAsyncDisposable
    {
        /// <summary>
        /// Load the context into memory but still not do anything with it.
        /// This usually goes together with a loading screen to make the loading prettier
        /// </summary>
        Task Load();
        
        /// <summary>
        /// Must be called after the context has finished loading.
        /// Will take whatever the context loaded previously and engage its entry point logic
        /// </summary>
        Task Start();
    }
}