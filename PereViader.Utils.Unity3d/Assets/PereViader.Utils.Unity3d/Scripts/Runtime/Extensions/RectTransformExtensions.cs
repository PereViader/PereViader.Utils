using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class RectTransformExtensions
    {
        public static void SetAnchorCenter(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }

        public static void SetAnchorTopRight(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
        }
        
        public static void SetAnchorTopLeft(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
        }

        public static void SetAnchorBottomLeft(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
        }

        public static void SetAnchorBottomRight(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
        }
        
        public static void SetAnchorStretchFull(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
        }

        public static void SetAnchorStretchTop(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
        }

        public static void SetAnchorStretchBottom(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 0);
        }

        public static void SetAnchorStretchLeft(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 1);
        }

        public static void SetAnchorStretchRight(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = new Vector2(1, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
        }
        
        public static void SetPivotCenter(this RectTransform rectTransform)
        {
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public static void SetPivotTopRight(this RectTransform rectTransform)
        {
            rectTransform.pivot = new Vector2(1, 1);
        }

        public static void SetPivotTopLeft(this RectTransform rectTransform)
        {
            rectTransform.pivot = new Vector2(0, 1);
        }

        public static void SetPivotBottomRight(this RectTransform rectTransform)
        {
            rectTransform.pivot = new Vector2(1, 0);
        }

        public static void SetPivotBottomLeft(this RectTransform rectTransform)
        {
            rectTransform.pivot = new Vector2(0, 0);
        }
    }
}