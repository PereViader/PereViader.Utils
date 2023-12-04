using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.ApplicationContext
{
    /// <summary>
    /// An application context is whatever area of your application that has its own separate identity from the rest of the app.
    /// It provides several hooks in the execution of the application stack lifecycle to be able to implement whatever functionality needed.
    /// Feel free to add any further methods like preloading and use any pooling desired on instances of this interface.
    /// Those can be manually called by the user and serve to extend the functionality further with the custom needs the user might have.
    /// </summary>
    public interface IApplicationContext
    {
        /// <summary>
        /// Will load whatever the application context needs into memory but still not actually do anything the user needs to see.
        /// This is usually tied with loading screens so everything in here usually happens behind a loading screen.
        /// </summary>
        Task Load(CancellationToken cancellationToken);
        
        /// <summary>
        /// Will start the context at a proper time when the user can see what happens.
        /// This is usually tied with loading screens so when the loading screen is gone, this is triggered.
        /// </summary>
        Task Enter(CancellationToken cancellationToken);
        
        /// <summary>
        /// Called when the context stops being the active top most one.
        /// This is useful for example to keep some things loaded as the context is still alive so they can be reused later.
        /// </summary>
        Task Suspend(CancellationToken cancellationToken);
        
        /// <summary>
        /// Called when the context becomes the top most one again after being suspended.
        /// Do initialization like Enter but from a state where there might be some things already in memory.
        /// </summary>
        Task Resume(CancellationToken cancellationToken);
        
        /// <summary>
        /// Called when the context is removed from the context stack.
        /// </summary>
        Task Exit(CancellationToken cancellationToken);
    }
}