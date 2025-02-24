using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.TaskRunners;
using PereViader.Utils.Unity3d.Extensions;
using PereViader.Utils.Unity3d.UiStack;
using UnityEngine;

namespace PereViader.Utils.Unity3d.Samples.UiStack
{
    public class UiStackSample : MonoBehaviour
    {
        public UiStackSampleScreen Screen1Prefab;
        public UiStackSampleScreen Screen2Prefab;
        public Transform UiStackRootTransform;

        private UiStackService _uiStackService;
        private UiStackSampleScreen _screen1;
        private UiStackSampleScreen _screen2;
        private TaskRunner _taskRunner = new();

        public void Start()
        {
            _uiStackService = new UiStackService(UiStackRootTransform, UiStackLayer.CreateDefaultLayers());

            _screen1 = Object.Instantiate(Screen1Prefab);
            _uiStackService.Register(_screen1.UiStackElement);
            
            _screen2 = Object.Instantiate(Screen2Prefab);
            _uiStackService.Register(_screen2.UiStackElement);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _taskRunner
                    .RunSequenced(ct => HideAndShow(_screen2.UiStackElement, _screen1.UiStackElement, ct))
                    .RunAsync();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _taskRunner
                    .RunSequenced(ct => HideAndShow(_screen1.UiStackElement, _screen2.UiStackElement, ct))
                    .RunAsync();
            }
        }
        
        //We are always hiding the previous screen and then showing the next one
        //This is however not necessary you may show a popup and not hide the previous screen thus placing the popup on top
        async Task HideAndShow(UiStackElement hide, UiStackElement show, CancellationToken ct)
        {
            await _uiStackService.Show(show, false, ct);
            await _uiStackService.Hide(hide, false, ct);
        }
        
        void OnDestroy()
        {
            _taskRunner.Dispose();
        }
    }
}