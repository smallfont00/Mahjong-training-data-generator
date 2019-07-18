using System;
using Process = System.Diagnostics.Process;
using UnityEngine;

public class Python : MonoBehaviour
{
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public void callPython(string loadPath, string savePath)
    {
        Process p = new Process();
        string desktop = $"{Environment.GetEnvironmentVariable("USERPROFILE")}/Desktop"; 
        string sArgName = "main.py";
        string sArguments = $"\"{desktop}/{sArgName}\""; //加引號是避免中間有空格會影響
        //string path = @"C:\Users\chsu\Desktop\" + sArgName;
        //string sArguments = scriptPath;
        p.StartInfo.FileName = "python.exe";
        sArguments += $" {loadPath} {savePath} - u";
        Debug.Log(sArguments);
        p.StartInfo.Arguments = sArguments;
        p.StartInfo.UseShellExecute = false;       // 必需
        p.StartInfo.RedirectStandardOutput = true; // 輸出引數設定
        p.StartInfo.RedirectStandardInput = true;  // 傳入引數設定
        p.Start();
        p.WaitForExit();
    }
}
