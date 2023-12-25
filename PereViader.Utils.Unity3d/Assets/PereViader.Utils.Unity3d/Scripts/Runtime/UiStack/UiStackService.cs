using System.Collections.Generic;
using PereViader.Utils.Common.ActiveStatuses;
using PereViader.Utils.Common.Attributes;
using PereViader.Utils.Unity3d.Extensions;
using UnityEngine;

namespace PereViader.Utils.Unity3d.UiStack
{
    [Experimental]
    public sealed class UiStackService : IUiStackService
    {
        public ActiveStatus InteractableActiveStatus { get; } = new ActiveStatus(defaultActiveState: true);

        public IReadOnlyDictionary<UiStackLayer, ActiveStatus> LayerInteractableActiveStatus { get; }

        private readonly Dictionary<UiStackLayer, Transform> _layerParents = new ();
        private readonly Dictionary<UiStackElement, Transform> _uiStackElementFormerParents = new ();
        
        public UiStackService(Transform rootTransform, IReadOnlyList<UiStackLayer> uiStackLayers)
        {
            var layerActiveStatus = new Dictionary<UiStackLayer, ActiveStatus>();
            LayerInteractableActiveStatus = layerActiveStatus;
            
            foreach (var uiStackLayer in uiStackLayers)
            {
                var gameObject = new GameObject(uiStackLayer.Name);
                var transform = gameObject.AddComponent<RectTransform>();
                transform.SetAnchorStretchFull();
                transform.SetPivotCenter();
                transform.SetParent(rootTransform, false);
                _layerParents.Add(uiStackLayer, transform);

                var activeStatus = new ActiveStatus(true);
                layerActiveStatus.Add(uiStackLayer, activeStatus);
                var layerCanvasGroup = transform.AddComponent<CanvasGroup>();
                layerCanvasGroup.LinkInteractableToActiveStatus(activeStatus);
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
        }

        public void Unregister(UiStackElement uiStackElement)
        {
            var formerParent = _uiStackElementFormerParents[uiStackElement];
            
            uiStackElement.Transform.SetParent(formerParent, worldPositionStays: false);

            _uiStackElementFormerParents.Remove(uiStackElement);
        }
    }
}