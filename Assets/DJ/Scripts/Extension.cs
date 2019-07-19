using UnityEngine;
using UnityObject = UnityEngine.Object;
using Path = System.IO.Path;
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
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                int id = Random.Range(i, array.Length);
                T temp = array[i];
                array[i] = array[id];
                array[id] = temp;
            }
        }
        public static string GetPath(params string[] paths)
        {
            string path = Path.Combine(paths);
            return Path.GetFullPath(path.Length == 0 ? "./" : path);
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
