using NUnit.Framework;
using PereViader.Utils.Common.ActiveStatuses;
using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions.Test
{
    public class TestCanvasGroupExtensions
    {
        [Test]
        public void LinkInteractableToActiveStatus()
        {
            GameObject go = new();
            var canvasGroup = go.AddComponent<CanvasGroup>();

            canvasGroup.interactable = false;
            
            ActiveStatus activeStatus = new(true);
            canvasGroup.LinkInteractableToActiveStatus(activeStatus);
            Assert.That(canvasGroup.interactable, Is.True);
            
            activeStatus.SetActive(new object(), false);
            Assert.That(canvasGroup.interactable, Is.False);
            
            activeStatus.ForgetAll();
            Assert.That(canvasGroup.interactable, Is.True);
        }
    }
}