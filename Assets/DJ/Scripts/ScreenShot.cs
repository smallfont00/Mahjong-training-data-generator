using System;
using System.IO;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    private int seconds = 0;
    private float time = 0;
    private bool isshot = false;
    private StreamWriter fs;
    private void Awake()
    {
        fs = new StreamWriter(Path.GetFullPath(Application.dataPath + "/../log.log"), false);
        var args = Environment.GetCommandLineArgs();
        Resolution res = Screen.currentResolution;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-screen-width")
            {
                i++;
                if (i < args.Length)
                {
                    if (int.TryParse(args[i], out int width))
                    {
                        res.width = width;
                    }
                }
            }
            else if (args[i] == "-screen-height")
            {
                i++;
                if (i < args.Length)
                {
                    if (int.TryParse(args[i], out int height))
                    {
                        res.height = height;
                    }
                }
            }
        }
        Screen.SetResolution(res.width, res.height, false);
    }
    void Start()
    {
        fs.WriteLine("Start");
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            while (time > 1)
            {
                seconds++;
                time -= 1;
            }
        }
        if (!isshot && seconds > 10)
        {
            fs.WriteLine("Attr Width: {0}, Height: {1}", Screen.width, Screen.height);
            fs.WriteLine("Rect Width: {0}, Height: {1}", Screen.currentResolution.width, Screen.currentResolution.height);
            fs.WriteLine(Path.GetFullPath(Application.dataPath + "/../test.jpg"));
            isshot = true;
            CaptureCamera(Camera.main);
            fs.Close();
            Application.Quit(0);
        }
    }

    private void CaptureCamera(Camera cam)
    {
        RenderTexture render = new RenderTexture(Screen.width, Screen.height, 0);
        RenderTexture.active = render;
        cam.targetTexture = render;
        cam.Render();
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();
        cam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(render);
        byte[] bytes = texture.EncodeToJPG();
        string filePath = Path.GetFullPath(Application.dataPath + "/../test.jpg");
        File.WriteAllBytes(filePath, bytes);
    }
}
