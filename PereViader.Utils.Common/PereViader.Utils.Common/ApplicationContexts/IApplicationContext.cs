using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.ApplicationContexts
{
    /// <summary>
    /// An application context is whatever area of your application that has its own separate identity from the rest of the app.
    /// It provides several hooks in the execution of the application stack lifecycle to be able to implement whatever functionality needed.
    /// Feel free to add any further methods like preloading and use any pooling desired on instances of this interface.
    /// Those can be manually called by the user and serve to extend the functionality further with the custom needs the user might have.
    /// </summary>
    public interface IApplicationContext : IAsyncDisposable
    {
        /// <summary>
        /// Will load whatever the application context needs into memory but still not actually do anything the user needs to see.
        /// This is usually tied with loading screens so everything in here usually happens behind a loading screen.
        /// </summary>
        Task Load();
        
        /// <summary>
        /// Will start the context at a proper time when the user can see what happens.
        /// This is usually tied with loading screens so when the loading screen is gone, this is triggered.
        /// </summary>
        Task Start();
    }
}