using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class ComponentExtensions
    {
        public static void DestroyComponent(this Component component)
        {
            Object.Destroy(component);
        }
        
        public static void DestroyGameObject(this Component component)
        {
            Object.Destroy(component.gameObject);
        }

        public static RectTransform UnsafeGetRectTransform(this Component component)
        {
            return (RectTransform)component.transform;
        }
        
        public static T GetOrAddComponent<T>(this Component component)
            where T : Component
        {
            var result = component.GetComponent<T>();
            if (result == null)
            {
                result = component.AddComponent<T>();
            }

            return result;
        }
        
        public static T AddComponent<T>(this Component component)
            where T : Component
        {
            return component.gameObject.AddComponent<T>();
        }
    }
}