using System.Threading;
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
                _uiStackService.InteractableActiveStatus.ToggleActive(this);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _uiStackService.LayerInteractableActiveStatus[UiStackLayer.DefaultLayer].ToggleActive(this);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _uiStackService.Show(_screen1.UiStackElement, false, CancellationToken.None).RunAsync();
                _uiStackService.Hide(_screen2.UiStackElement, false, CancellationToken.None).RunAsync();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                _uiStackService.Show(_screen2.UiStackElement, false, CancellationToken.None).RunAsync();
                _uiStackService.Hide(_screen1.UiStackElement, false, CancellationToken.None).RunAsync();
            }
        }
    }
}