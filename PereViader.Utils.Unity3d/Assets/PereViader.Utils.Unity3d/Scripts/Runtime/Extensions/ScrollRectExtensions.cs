using PereViader.Utils.Common.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class ScrollRectExtensions
    {
        public static void SetHorizontalScrollLeft(this ScrollRect scrollRect)
        {
            scrollRect.horizontalNormalizedPosition = 0f;
        }
        
        public static void SetHorizontalScrollRight(this ScrollRect scrollRect)
        {
            scrollRect.horizontalNormalizedPosition = 1f;
        }
        
        public static void SetHorizontalScrollTop(this ScrollRect scrollRect)
        {
            scrollRect.verticalNormalizedPosition = 1f;
        }
        
        public static void SetHorizontalScrollBottom(this ScrollRect scrollRect)
        {
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public static void SetElementNormalizedPosition(
            this ScrollRect scrollRect,
            RectTransform rectTransform,
            Vector2 normalizedViewportPosition)
        {
            scrollRect.normalizedPosition =
                scrollRect.GetElementNormalizedPosition(rectTransform, normalizedViewportPosition);
        }
        
        public static Vector2 GetElementNormalizedPosition(
            this ScrollRect scrollRect,
            RectTransform rectTransform,
            Vector2 normalizedViewportPosition)
        {
            var position = scrollRect.content.InverseTransformPoint(rectTransform.position);
            var min = scrollRect.content.rect.min;
            var max = min + scrollRect.GetScrollableSize();
            var windowSize = scrollRect.viewport.rect.size;
            
            var x = MathExtensions.NormalizeClampedInWindow(
                position.x, 
                min.x, 
                max.x, 
                windowSize.x, 
                normalizedViewportPosition.x);
            
            var y = MathExtensions.NormalizeClampedInWindow(
                position.y, 
                min.y, 
                max.y, 
                windowSize.y, 
                1f - normalizedViewportPosition.y);

            return new Vector2(x, y);
        }

        public static void SetElementHorizontalNormalizedPosition(
            this ScrollRect scrollRect,
            RectTransform rectTransform,
            float normalizedViewportPosition)
        {
            scrollRect.horizontalNormalizedPosition = scrollRect.GetElementHorizontalNormalizedPosition(
                rectTransform,
                normalizedViewportPosition);
        }
        
        public static float GetElementHorizontalNormalizedPosition(
            this ScrollRect scrollRect,
            RectTransform rectTransform,
            float normalizedViewportPosition)
        {
            var position = scrollRect.content.InverseTransformPoint(rectTransform.position);
            var min = scrollRect.content.rect.xMin;
            var max = min + scrollRect.GetScrollableWidth();
            var windowSize = scrollRect.viewport.rect.width;
            
            return MathExtensions.NormalizeClampedInWindow(
                position.x, 
                min, 
                max, 
                windowSize, 
                normalizedViewportPosition);
        }

        public static void SetElementVerticalNormalizedPosition(
            this ScrollRect scrollRect,
            RectTransform rectTransform,
            float normalizedViewportPosition)
        {
            scrollRect.verticalNormalizedPosition = scrollRect.GetElementVerticalNormalizedPosition(
                rectTransform,
                normalizedViewportPosition);
        }
        
        public static float GetElementVerticalNormalizedPosition(
            this ScrollRect scrollRect,
            RectTransform rectTransform,
            float normalizedViewportPosition)
        {
            var position = scrollRect.content.InverseTransformPoint(rectTransform.position);
            var min = scrollRect.content.rect.yMin;
            var max = min + scrollRect.GetScrollableHeight();
            var windowSize = scrollRect.viewport.rect.height;
            
            return MathExtensions.NormalizeClampedInWindow(
                position.y, 
                min,
                max,
                windowSize,
                1f - normalizedViewportPosition);
        }
        
        public static float GetScrollableWidth(this ScrollRect scrollRect)
        {
            var viewportSize = scrollRect.viewport.rect.width;
            var contentSize = scrollRect.content.rect.width;
            return Mathf.Max(0f, contentSize - viewportSize);
        }
        
        public static float GetScrollableHeight(this ScrollRect scrollRect)
        {
            var viewportSize = scrollRect.viewport.rect.height;
            var contentSize = scrollRect.content.rect.height;
            return Mathf.Max(0f, contentSize - viewportSize);
        }

        public static Vector2 GetScrollableSize(this ScrollRect scrollRect)
        {
            var viewportSize = scrollRect.viewport.rect.size;
            var contentSize = scrollRect.content.rect.size;
            return Vector2.Max(Vector2.zero, contentSize - viewportSize);
        }
    }
}