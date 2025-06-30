using System;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.ApplicationContexts;
using UnityEngine.SceneManagement;
using TaskExtensions = PereViader.Utils.Common.Extensions.TaskExtensions;


namespace PereViader.Utils.Unity3d.ApplicationContexts
{
    /// <summary>
    /// Base IApplicationContext for loading and unloading scenes using the SceneManager async api  
    /// </summary>
    public abstract class SceneApplicationContext : IApplicationContext
    {
        public abstract string SceneName { get; }
        public virtual LoadSceneMode LoadSceneMode => LoadSceneMode.Additive;
        public virtual UnloadSceneOptions UnloadSceneOptions => UnloadSceneOptions.None;
        public Scene? Scene { get; private set; }
        
        public virtual async Task Load(CancellationToken ct)
        {
            var nextSceneIndex = SceneManager.sceneCount;
            var asyncOperation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode);
            if (asyncOperation is null)
            {
                throw new InvalidOperationException($"Could not load scene {SceneName}"); 
            }
            Scene = SceneManager.GetSceneAt(nextSceneIndex);
            await TaskExtensions.WaitUntil(() => asyncOperation.isDone, ct);
        }

        public virtual Task Start(CancellationToken ct)
        {
            return Task.CompletedTask;
        }
        
        public async ValueTask DisposeAsync()
        {
            if (!Scene.HasValue)
            {
                return;
            }
            var asyncOperation = SceneManager.UnloadSceneAsync(Scene.Value, UnloadSceneOptions);
            Scene = null;
            if (asyncOperation is null)
            {
                throw new InvalidOperationException($"Could not unload scene {SceneName}"); 
            }
            await TaskExtensions.WaitUntil(() => asyncOperation.isDone, CancellationToken.None);
        }
    }
}