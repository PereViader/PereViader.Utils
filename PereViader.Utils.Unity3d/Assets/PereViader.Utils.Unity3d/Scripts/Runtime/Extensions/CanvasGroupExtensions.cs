using PereViader.Utils.Common.ActiveStatuses;
using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class CanvasGroupExtensions
    {
        public static void LinkInteractableToActiveStatus(this CanvasGroup canvasGroup, ActiveStatus activeStatus)
        {
            canvasGroup.interactable = activeStatus.IsActive();
            activeStatus.OnChanged += x => canvasGroup.interactable = x;
        }
    }
}