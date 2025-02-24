using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.ApplicationContexts;
using PereViader.Utils.Common.TaskRunners;
using PereViader.Utils.Unity3d.ApplicationContexts;
using PereViader.Utils.Unity3d.Extensions;
using UnityEngine;

// SceneApplicationContext can be used as a base class and provides a basic functionality to load scenes
// But you can make custom implementations of the interface with your specific requirements
// For example loading assets  from addressables, or even building scenes dynamically
public class Scene1ApplicationContext : SceneApplicationContext 
{
    public override string SceneName { get; } = "ApplicationContextScene1";

    public override Task Load()
    {
        //Some dummy logic to showcase you can do further stuff
        Debug.Log("Scene will now load");
        return base.Load();
    }

    public override Task Start()
    {
        Debug.Log("Scene 1 is now loaded and you can do things with it for example get references to the scene objects");
        return base.Start();
    }
}

public class Scene2ApplicationContext : SceneApplicationContext
{
    public override string SceneName { get; } = "ApplicationContextScene2";
}

public class ApplicationContextsSample : MonoBehaviour
{
    private readonly ApplicationContextService _applicationContextService = new();
    private readonly TaskRunner _taskRunner = new();

    private IApplicationContextHandle _currentApplicationContextHandle;
    
    public void Update()
    {
        if (Input.anyKeyDown)
        {
            _taskRunner
                .RunSequenced(ToggleScene)
                .RunAsync();
        }
    }
        
    //In this example We are always hiding the previous screen and then showing the next one.
    //In a real game, you may have more than one IApplicationContext active at the same time.
    //For example you may have an application context for a menu and another application context for a popup that the menu opened
    //The menu will be the first loaded and started and the popup one will be loaded and started from within the menu
    async Task ToggleScene(CancellationToken ct)
    {
        // This is just some dummy logic to Toggle between contexts
        var nextApplicationContext = _currentApplicationContextHandle is null or { ApplicationContext: Scene2ApplicationContext }
            ? (IApplicationContext) new Scene1ApplicationContext()
            : new Scene2ApplicationContext();

        var previousApplicationContext = _currentApplicationContextHandle;
        _currentApplicationContextHandle = _applicationContextService.Add(nextApplicationContext);

        //With this pattern it is trivial to add a loading screen that hides the loading and disposal of application contexts 
        await _currentApplicationContextHandle.Load();
        
        // We could run loading and unloading in parallel if we used Task.WhenAll
        if (previousApplicationContext != null)
        {
            await previousApplicationContext.DisposeAsync();
        }
        
        await _currentApplicationContextHandle.Start();
    }
        
    void OnDestroy()
    {
        _taskRunner.Dispose();
        _applicationContextService.Dispose();
    }
}
