namespace PereViader.Utils.Unity3d.UiStack
{
    public class UiStackLayer
    {
        public static readonly UiStackLayer DefaultLayer = new ("Default");
        
        public string Name { get; }

        public UiStackLayer(string name)
        {
            Name = name;
        }

        public static UiStackLayer[] CreateDefaultLayers() => new [] { DefaultLayer };
    }
}