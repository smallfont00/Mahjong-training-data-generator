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
        string sArgName = "main.py";
        string path = @"C:\Users\chsu\Desktop\" + sArgName;
        string sArguments = path;
        p.StartInfo.FileName = "python.exe";
        loadPath += "*.jpg";
        sArguments += " " + loadPath + " " + savePath;
        UnityEngine.Debug.Log(sArguments);
        p.StartInfo.Arguments = sArguments;
        p.StartInfo.UseShellExecute = false; //必需
        p.StartInfo.RedirectStandardOutput = true;//輸出引數設定
        p.StartInfo.RedirectStandardInput = true;//傳入引數設定
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        string output = p.StandardOutput.ReadToEnd();
        UnityEngine.Debug.Log(output);
        p.WaitForExit();
    }
}
