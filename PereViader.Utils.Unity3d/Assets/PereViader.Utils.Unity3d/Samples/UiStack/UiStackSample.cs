using System.Threading.Tasks;
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
        
        public void Start()
        {
            _uiStackService = new UiStackService(UiStackRootTransform, UiStackLayer.CreateDefaultLayers());

            var screen1 = Object.Instantiate(Screen1Prefab);
            _uiStackService.Register(new UiStackElement(UiStackLayer.DefaultLayer, screen1.transform, (visible, instantly, ct) =>
            {
                screen1.gameObject.SetActive(visible);
                return Task.CompletedTask;
            }));
            
            var screen2 = Object.Instantiate(Screen2Prefab);
            _uiStackService.Register(new UiStackElement(UiStackLayer.DefaultLayer, screen2.transform, (visible, instantly, ct) =>
            {
                screen2.gameObject.SetActive(visible);
                return Task.CompletedTask;
            }));
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _uiStackService.InteractableActiveStatus.Toggle(this);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _uiStackService.LayerInteractableActiveStatus[UiStackLayer.DefaultLayer].Toggle(this);
            }
        }
    }
}