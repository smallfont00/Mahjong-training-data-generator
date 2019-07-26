using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class EditorTry : MonoBehaviour
{
    public string path = "test~";
    public void Play()
    {
        var list = Resources.LoadAll<Texture2D>(path);
        Debug.Log(list.Length);
        foreach(var i in list)
        {
            Debug.Log(i);
        }
    }
}

[CustomEditor(typeof(EditorTry))]
public class AddButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorTry MyScript = (EditorTry)target;

        if (GUILayout.Button("Button"))
        {
            MyScript.Play();
        }

    }
}
#endif
