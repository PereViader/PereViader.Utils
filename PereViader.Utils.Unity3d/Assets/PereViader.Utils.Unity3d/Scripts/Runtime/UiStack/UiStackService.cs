using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.ActiveStatuses;
using PereViader.Utils.Common.Attributes;
using PereViader.Utils.Common.TaskRunners;
using PereViader.Utils.Unity3d.Extensions;
using UnityEngine;

namespace PereViader.Utils.Unity3d.UiStack
{
    [Experimental]
    public sealed class UiStackService : IUiStackService
    {
        public ActiveStatus InteractableActiveStatus { get; } = new ActiveStatus(defaultActiveState: true);

        public IReadOnlyDictionary<UiStackLayer, ActiveStatus> LayerInteractableActiveStatus { get; }

        public IReadOnlyDictionary<UiStackLayer, IReadOnlyCollection<UiStackElement>> UiStackElements { get; }
        
        private readonly Dictionary<UiStackLayer, Transform> _layerParents = new ();
        private readonly Dictionary<UiStackElement, Transform> _uiStackElementFormerParents = new ();
        private readonly TaskRunner _taskRunner = new TaskRunner();

        public UiStackService(Transform rootTransform, IReadOnlyList<UiStackLayer> uiStackLayers)
        {
            var uiStackElements = new Dictionary<UiStackLayer, IReadOnlyCollection<UiStackElement>>();
            UiStackElements = uiStackElements;

            var layerActiveStatus = new Dictionary<UiStackLayer, ActiveStatus>();
            LayerInteractableActiveStatus = layerActiveStatus;
            
            foreach (var uiStackLayer in uiStackLayers)
            {
                var gameObject = new GameObject(uiStackLayer.Name);
                var transform = gameObject.AddComponent<RectTransform>();
                transform.SetAnchorStretchFull();
                transform.SetPivotCenter();
                transform.SetParent(rootTransform, false);
                gameObject.isStatic = true;

                _layerParents.Add(uiStackLayer, transform);

                var activeStatus = new ActiveStatus(true);
                layerActiveStatus.Add(uiStackLayer, activeStatus);
                var layerCanvasGroup = transform.AddComponent<CanvasGroup>();
                layerCanvasGroup.LinkInteractableToActiveStatus(activeStatus);
                
                uiStackElements.Add(uiStackLayer, new HashSet<UiStackElement>());
            }

            var canvasGroup = rootTransform.GetOrAddComponent<CanvasGroup>();
            canvasGroup.LinkInteractableToActiveStatus(InteractableActiveStatus);
        }

        public void Register(UiStackElement uiStackElement)
        {
            var layerParent = _layerParents[uiStackElement.UiStackLayer];
            
            _uiStackElementFormerParents[uiStackElement] = uiStackElement.Transform.parent;
            
            uiStackElement.Transform.SetParent(layerParent, worldPositionStays: false);
            uiStackElement.Transform.SetAsFirstSibling();

            var layerElements = (HashSet<UiStackElement>)UiStackElements[uiStackElement.UiStackLayer];
            layerElements.Add(uiStackElement);
        }

        public void Unregister(UiStackElement uiStackElement)
        {
            var formerParent = _uiStackElementFormerParents[uiStackElement];
            
            uiStackElement.Transform.SetParent(formerParent, worldPositionStays: false);

            _uiStackElementFormerParents.Remove(uiStackElement);
            
            var layerElements = (HashSet<UiStackElement>)UiStackElements[uiStackElement.UiStackLayer];
            layerElements.Remove(uiStackElement);
        }

        public Task Show(UiStackElement uiStackElement, bool instantly, CancellationToken cancellationToken)
        {
            return SetVisible(uiStackElement, true, instantly, cancellationToken);
        }

        public Task Hide(UiStackElement uiStackElement, bool instantly, CancellationToken cancellationToken)
        {
            return SetVisible(uiStackElement, false, instantly, cancellationToken);
        }

        public Task SetVisible(UiStackElement uiStackElement, bool visible, bool instantly, CancellationToken cancellationToken)
        {
            return _taskRunner.RunSequenced(
                (state, ct) => state.service.DoSetVisible(state.uiStackElement, state.visible, state.instantly, ct), 
                (uiStackElement, visible, instantly, service: this),
                cancellationToken);
        }

        private async Task DoSetVisible(UiStackElement uiStackElement, bool visible, bool instantly,
            CancellationToken cancellationToken)
        {
            if (visible)
            {
                uiStackElement.Transform.SetAsLastSibling();
            }

            await uiStackElement.SetUiStackElementVisibleDelegate(visible, instantly, cancellationToken);
        }
    }
}