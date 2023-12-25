using UnityEngine;

namespace PereViader.Utils.Unity3d.UiStack
{
    public sealed class UiStackElement
    {
        public UiStackLayer UiStackLayer { get; }
        public Transform Transform { get; }
        public SetUiStackElementVisibleDelegate SetUiStackElementVisibleDelegate { get; }

        public UiStackElement(UiStackLayer uiStackLayer, Transform transform, SetUiStackElementVisibleDelegate setUiStackElementVisibleDelegate)
        {
            UiStackLayer = uiStackLayer;
            Transform = transform;
            SetUiStackElementVisibleDelegate = setUiStackElementVisibleDelegate;
        }
    }
}