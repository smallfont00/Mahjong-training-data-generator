using UnityEngine;
using UnityObject = UnityEngine.Object;
namespace CommandLineTest
{
    public static partial class Ext
    {
        public static void RemoveAllChild(this Transform transform)
        {
            for (int i = transform.childCount; i > 0; i--)
            {
                UnityObject.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
    public sealed class CustomMinAttribute : PropertyAttribute
    {
        public readonly object min;
        public CustomMinAttribute(float value)
        {
            min = value;
        }
        public CustomMinAttribute(int value)
        {
            min = value;
        }
        public T Get<T>()
        {
            return min is T v ? v : default;
        }
    }
}
