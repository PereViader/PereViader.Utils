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

        /// <summary>
        /// This only includes the default layer.
        /// If you need more layers, create a new method with the necessary ones.
        /// </summary>
        public static UiStackLayer[] CreateDefaultLayers() => new [] { DefaultLayer };
    }
}