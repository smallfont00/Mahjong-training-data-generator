using System;
using System.IO;
using UnityEngine;
using CommandLineTest;
public partial class Screenshot : MonoBehaviour
{
    [CustomMin(0)]
    public int ShotCount = 0;
    public DrawYolo Controller;
    private void Awake()
    {
        Screen.SetResolution(1024, 768, false);
    }

    private string CaptureCamera(Camera cam, string path, int id)
    {
        RenderTexture render = new RenderTexture(Screen.width, Screen.height, 0);
        RenderTexture.active = render;
        cam.targetTexture = render;
        cam.Render();
        Texture2D texture = new Texture2D(render.width, render.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        texture.Apply();
        cam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(render);
        byte[] bytes = texture.EncodeToJPG();
        string fileName = id + ".jpg";
        string filePath = Ext.GetPath(Path.Combine(path, fileName));
        File.WriteAllBytes(filePath, bytes);
        return "Image/" + fileName;
    }
    public void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "CaptureScreen"))
        {
            var detector = GetComponent<CamDetect>();
            string timeString = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss");
            Path.Combine();
            string datafolder = Ext.GetPath(Application.dataPath, "ScreenShot~", timeString, "Image");
            Debug.Log(datafolder);
            Directory.CreateDirectory(datafolder);
            using (var sw = new StreamWriter(Ext.GetPath(datafolder, "..", "train.txt")))
            {
                for (int i = 0; i < ShotCount; i++)
                {
                    Controller.Next();
                    string imagePath = CaptureCamera(Camera.main, datafolder, i);
                    var yoloData = detector.GetDetectedMjYoloData();
                    sw.Write(imagePath);
                    foreach (var j in yoloData)
                    {
                        sw.Write(" " + j);
                    }
                    sw.WriteLine();
                }
            }
            Resources.UnloadUnusedAssets();
        }
    }
}
