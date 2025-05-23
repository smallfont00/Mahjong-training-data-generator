﻿using System.Collections;
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
        string sArgName = "/main.py";
        string path = Application.dataPath + sArgName;
        string sArguments = path;
        p.StartInfo.FileName = "C:/Users/acer/Anaconda3/envs/py36/python.exe";
        sArguments += " " + loadPath + " " + savePath + " - u";
        UnityEngine.Debug.Log(sArguments);
        p.StartInfo.Arguments = sArguments;
        p.StartInfo.UseShellExecute = false; //必需
        p.StartInfo.RedirectStandardOutput = true;//輸出引數設定
        p.StartInfo.RedirectStandardInput = true;//傳入引數設定
        //p.StartInfo.CreateNoWindow = true;
        p.Start();
        p.WaitForExit();
    }
}
