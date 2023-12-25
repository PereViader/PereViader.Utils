using UnityEngine;

namespace PereViader.Utils.Unity3d.Extensions
{
    public static class GameObjectExtensions
    {
        public static void DestroyGameObject(this GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }

        public static RectTransform GetRectTransform(this GameObject gameObject)
        {
            return (RectTransform)gameObject.transform;
        }
        
        public static T GetOrAddComponent<T>(this GameObject gameObject)
            where T : Component
        {
            var result = gameObject.GetComponent<T>();
            if (result == null)
            {
                result = gameObject.AddComponent<T>();
            }

            return result;
        }
    }
}