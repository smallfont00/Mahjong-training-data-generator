using UnityEngine;
using UnityEditor;
namespace CommandLineTest
{
    [CustomEditor(typeof(CreateMJ))]
    public class CreateMJEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (target is CreateMJ myScript)
            {
                if (GUILayout.Button("Create"))
                {
                    myScript.CreateObjects();
                }
            }
        }
    }

    [CustomPropertyDrawer(typeof(CustomMinAttribute))]
    public class CustomMinDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is CustomMinAttribute min)
            {
                if (property.propertyType == SerializedPropertyType.Float)
                {
                    float value = EditorGUI.FloatField(position, label, property.floatValue);
                    property.floatValue = Mathf.Max(min.Get<float>(), value);
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    int value = EditorGUI.IntField(position, label, property.intValue);
                    property.intValue = Mathf.Max(min.Get<int>(), value);
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Use Min with float or int.");
                }
            }
        }
    }
}

