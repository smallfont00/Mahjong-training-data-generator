using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System;

public class Python : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void callPython(string loadPath, string savePath)
    {
        Process p = new Process();
        string path = Path.Combine(Application.dataPath, "..", "main.py");
        string sArguments = $"{path} \"{Path.Combine(loadPath, "*.jpg")}\" \"{savePath}\"";
        p.StartInfo.FileName = "python.exe";
        p.StartInfo.Arguments = sArguments;
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        p.WaitForExit();
    }
}