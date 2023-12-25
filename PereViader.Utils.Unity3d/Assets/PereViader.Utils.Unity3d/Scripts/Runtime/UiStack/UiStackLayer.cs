namespace PereViader.Utils.Unity3d.UiStack
{
    public class UiStackLayer
    {
        public static readonly UiStackLayer DefaultLayer = new ("Default");
        public static readonly UiStackLayer PopupLayer = new("Popup");
        public static readonly UiStackLayer LoadingLayer = new("Loading");
        
        public string Name { get; }

        public UiStackLayer(string name)
        {
            Name = name;
        }

        public static UiStackLayer[] CreateDefaultLayers() => new [] { DefaultLayer, PopupLayer, LoadingLayer };
    }
}