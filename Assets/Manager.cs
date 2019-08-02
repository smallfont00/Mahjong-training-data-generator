using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Manager : MonoBehaviour
{

    private static string folderName = "";

    // Start is called before the first frame update
    public GameObject[] cameras;

    //public Camera[] cameras;
    private List<GameObject> mjBuilder = new List<GameObject>();

    //public GameObject[] mjBuilder;
    static string[] resourcesPath = new[] { "type1/", "type2/", "type3/", "type4/", "type5/", "type6/" };
    Python python;
    void Start()
    {
        python = GameObject.Find("Python").GetComponent<Python>();
        foreach (Transform child in transform)
        {
            var mj = child.gameObject.GetComponent<Array>()?.gameObject;
            if (mj) { mjBuilder.Add(mj); }
        }
        
        foreach (GameObject cam in cameras) cam.GetComponent<CameraDetect>().SetManager(this);
    }
    public GameObject[] GetManagerMj()
    {
        List<GameObject> mjs = new List<GameObject>();
        mjBuilder.ForEach(mjb => { foreach (Transform mj in mjb.transform) { mjs.Add(mj.gameObject); } });
        return mjs.ToArray();
    }

    private IEnumerator m_Coroutine;

    public void StartTakeScreenShot(int start = 0, int end = 0)
    {
        if (m_Coroutine != null)
        {
            Debug.Log("Screen Shot Program Is Still Running");
            return;
        }
        m_Coroutine = RunScreenShotProgram(start, end);
        StartCoroutine(m_Coroutine);
    }

    public void StopTakeScreenShot()
    {
        StopCoroutine(m_Coroutine);
        m_Coroutine = null;
        folderName = "";
        Debug.Log("Screen Shot Program Is Stop");
    }

    public IEnumerator RunScreenShotProgram(int start = 0, int end = 0)
    {
        folderName = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
        for (int i = start; i < end; i++)
        {
            foreach (GameObject mjb in mjBuilder)
            {
                mjb.GetComponent<Array>().ChangeRightNumberRandomly();
            }
            foreach (GameObject mjb in mjBuilder)
            {
                mjb.GetComponent<Array>().ChangeAllMjStyleRandomly();
            }

            ShakeCameras();

            //foreach (Camera camera1 in FindObjectsOfType<Camera>())
            //{
            //    camera1.orthographic = (Random.Range(0, 2) == 1);
            //}
            var cam = cameras[Random.Range(0, cameras.Length)];
            yield return new WaitForEndOfFrame();
            TakeScreenShot(cam, i);
        }
        yield return null;
        StopTakeScreenShot();
    }


    private void TakeScreenShot(GameObject cam, int screenShotNumber)
    {
        var camera = cam.GetComponent<Camera>();
        var detector = cam.GetComponent<CameraDetect>();

        var yoloData = detector.GetDetectedMjYoloData();
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);

        camera.targetTexture = rt;
        camera.Render();
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenShot.Apply();

        camera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        byte[] bytes = screenShot.EncodeToJPG();
        string filePath = Application.dataPath + "/ScreenShot~/" + folderName + "/Image/";
        string imageFileName = "p" + screenShotNumber + ".jpg";
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        //Debug.Log(filePath);
        System.IO.File.WriteAllBytes(filePath + imageFileName, bytes);
        List<string> labels = new List<string>();
        labels.Add("Image/" + imageFileName);
        foreach (YoloData data in yoloData)
        {
            Rect rect = data.Rect;
            var minX = (int)data.Rect.x;
            var minY = (int)data.Rect.y;
            var maxX = (int)(data.Rect.x + data.Rect.width);
            var maxY = (int)(data.Rect.y + data.Rect.height);
            string[] labelFeild =
            {
                    minX.ToString(), minY.ToString(), maxX.ToString(), maxY.ToString(), data.Type
                };
            labels.Add(String.Join(",", labelFeild));
        }

        using (var sw =
            System.IO.File.AppendText(Application.dataPath + "/ScreenShot~/" + folderName + "/train.txt"))
        {
            sw.WriteLine(String.Join(" ", labels));
        }
        screenShotNumber++;
        Resources.UnloadUnusedAssets();
    }

    public void ShakeCameras()
    {
        for (var i = 0; i < cameras.Length; i++)
        {
            cameras[i].GetComponent<CameraDetect>().RandomSlightlyTransform();
        }
    }

    private void OnGUI()
    {
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(Manager))]
public class ManagerEditor : Editor
{
    private Manager m_Manager;
    private int m_start_index = 0;
    private int m_end_index = 2500;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        m_Manager = (Manager)target;
        m_start_index = EditorGUILayout.IntField("Shot Number", m_start_index);
        m_end_index = EditorGUILayout.IntField("Shot Number", m_end_index);
        if (GUILayout.Button("Start Screen Shot"))
        {
            if (Application.isPlaying)
                m_Manager.StartTakeScreenShot(m_start_index, m_end_index);
            else
                Debug.Log("Please do it in play mode");
        }

        if (GUILayout.Button("Force Stop Screen Shot"))
        {
            m_Manager.StopTakeScreenShot();
        }
    }

    void PrintCurrentScreenSize()
    {
        Debug.Log(new Vector2(Screen.width, Screen.height));
    }
}
#endif