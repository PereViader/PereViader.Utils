﻿using System;
using System.Threading.Tasks;

namespace PereViader.Utils.Common.ApplicationContexts
{
    /// <summary>
    /// Wrapper around IApplicationContext that is used to represent an active context
    /// </summary>
    public interface IApplicationContextHandle : IAsyncDisposable
    {
        IApplicationContext ApplicationContext { get; }
        
        Task Load();
        Task Start();
    }
}