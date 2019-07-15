using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class EditorTry : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Awake");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
    }

    public void Play()
    {
        Debug.Log("Button");
    }
}

[CustomEditor(typeof(EditorTry))]
public class  AddButtonEditor : Editor
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
