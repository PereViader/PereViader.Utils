using PereViader.Utils.Unity3d.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PereViader.Utils.Unity3d.UI
{
    public class UpdateScrollRectOnSelected : MonoBehaviour, ISelectHandler
    {
        [Tooltip("Can be null, will use self")]
        public RectTransform RectTransform;
        public Vector2 NormalizedViewportPosition = Vector2Extensions.Half;

        public void Awake()
        {
            if (RectTransform == null)
            {
                RectTransform = this.UnsafeGetRectTransform();
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (eventData is not AxisEventData)
            {
                return;
            }
            
            var scrollRect = GetComponentInParent<ScrollRect>();
            if (scrollRect == null)
            {
                Debug.LogError("There was no ScrollRect to scroll", this);
            }

            scrollRect.normalizedPosition = scrollRect.GetElementNormalizedPosition(RectTransform, NormalizedViewportPosition);        
        }

        private void Reset()
        {
            RectTransform = this.UnsafeGetRectTransform();
        }
    }
}