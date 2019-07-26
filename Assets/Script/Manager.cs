using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Manager : MonoBehaviour
{
    private static int screenShotNumber = 0;

    private static string folderName = "";

    public GameObject[] cameras;
    public GameObject[] mjBuilder;
    private CameraDetect[] m_CameraDetectsScript;
    private Camera[] m_CamerasScript;
    static string[] resourcesPath = new[] { "type1/", "type2/", "type3/", "type4/", "type5/", "type6/" };
    Python python;
    void Start()
    {
        python = GameObject.Find("Python").GetComponent<Python>();
        m_CameraDetectsScript = new CameraDetect[cameras.Length];
        m_CamerasScript = new Camera[cameras.Length];
        for (var i = 0; i < cameras.Length; i++)
        {
            m_CameraDetectsScript[i] = cameras[i].GetComponent<CameraDetect>();
            m_CamerasScript[i] = cameras[i].GetComponent<Camera>();
        }
    }

    private IEnumerator m_Coroutine;

    public void StartTakeScreenshot(int number = 0)
    {
        if (m_Coroutine != null)
        {
            Debug.Log("Screen Shot Program Is Still Running");
        }
        else
        {
            foreach (var detect in m_CameraDetectsScript)
            {
                detect.DisableGUI = true;
            }
            m_Coroutine = RunScreenshotProgram(number);
            StartCoroutine(m_Coroutine);
        }
    }

    public void StopTakeScreenShot()
    {
        foreach (var detect in m_CameraDetectsScript)
        {
            detect.DisableGUI = false;
        }
        StopCoroutine(m_Coroutine);
        m_Coroutine = null;
        screenShotNumber = 0;
        folderName = "";
        Debug.Log("Screenshot Program Is Stop");
    }

    public IEnumerator RunScreenshotProgram(int number)
    {
        folderName = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
        for (int i = 0; i < number; i++)
        {
            if (i % 25 == 0)
            {
                var source = Path.Combine(Application.dataPath, "Mj", "Resources");
                for (int j = 0; j < 6; j++)
                {
                    string path = Path.Combine(source, resourcesPath[j]);
                    string savePath = Path.Combine(source, $"imgaug{j + 1}");
                    python.callPython(path, savePath);
                }
            }
            foreach (GameObject mjb in mjBuilder)
            {
                mjb.GetComponent<MJArray>().ChangeRightNumberRandomly();
            }
            foreach (GameObject mjb in mjBuilder)
            {
                mjb.GetComponent<MJArray>().ChangeAllMjStyleRandomly();
            }

            ShakeCameras();

            foreach (Camera camera1 in FindObjectsOfType<Camera>())
            {
                camera1.orthographic = Random.Range(0, 2) == 1;
            }
            yield return new WaitForEndOfFrame();
            TakeScreenshot();
        }
        yield return null;
        StopTakeScreenShot();
    }


    private void TakeScreenshot()
    {
        foreach (GameObject cam in cameras)
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
            string savePath = Path.Combine(Application.dataPath, "ScreenShot~", folderName);
            string filePath = Path.Combine(savePath, "Image");
            string fileName = $"p{screenShotNumber}.jpg";
            Directory.CreateDirectory(filePath);
            File.WriteAllBytes(Path.Combine(filePath, fileName), bytes);
            List<string> labels = new List<string> { Path.Combine("Image", fileName) };
            foreach (YoloData data in yoloData)
            {
                labels.Add(data.ToString());
            }

            using (var sw = File.AppendText(Path.Combine(savePath, "train.txt")))
            {
                sw.WriteLine(string.Join(" ", labels));
            }
            screenShotNumber++;
        }
        Resources.UnloadUnusedAssets();
    }

    public void ShakeCameras()
    {
        for (var i = 0; i < cameras.Length; i++)
        {
            cameras[i].GetComponent<CameraDetect>().RandomSlightlyTransform();
        }
    }
}