using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Manager))]
public class ManagerEditor : Editor
{
    private Manager m_Manager;
    private int m_image_number = 1000;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        m_image_number = EditorGUILayout.IntField("Shot Number", m_image_number);
        if (target is Manager m_Manager)
        {
            if (GUILayout.Button("Start Screenshot"))
            {
                if (Application.isPlaying)
                {
                    m_Manager.StartTakeScreenshot(m_image_number);
                }
                else
                {
                    Debug.Log("Please do it in play mode");
                }
            }
            if (GUILayout.Button("Force Stop Screenshot"))
            {
                m_Manager.StopTakeScreenShot();
            }
        }
    }
}