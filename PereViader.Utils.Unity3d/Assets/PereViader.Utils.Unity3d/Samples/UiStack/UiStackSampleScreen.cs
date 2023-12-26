using System.Threading.Tasks;
using PereViader.Utils.Unity3d.UiStack;
using UnityEngine;

namespace PereViader.Utils.Unity3d.Samples.UiStack
{
    public class UiStackSampleScreen : MonoBehaviour
    {
        public UiStackElement UiStackElement;

        public void Awake()
        {
            UiStackElement = new UiStackElement(UiStackLayer.DefaultLayer, transform, (visible, instantly, ct) =>
            {
                gameObject.SetActive(visible);
                return Task.CompletedTask;
            });
        }
    }
}
