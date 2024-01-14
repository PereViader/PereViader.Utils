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
        [Tooltip("Can be null, will use GetComponentInParent")]
        public ScrollRect ScrollRect;
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

            if (ScrollRect == null)
            {
                ScrollRect = GetComponentInParent<ScrollRect>();
            }
            
            if (ScrollRect == null)
            {
                Debug.LogError("Could not get ScrollRect to scroll", this);
            }

            ScrollRect.SetElementNormalizedPosition(RectTransform, NormalizedViewportPosition);        
        }

        private void Reset()
        {
            RectTransform = this.UnsafeGetRectTransform();
            ScrollRect = GetComponentInParent<ScrollRect>();
        }
    }
}