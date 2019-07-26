using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoloData
{
    public Rect Rect { get; }
    public string Type { get; }
    public YoloData(Rect rect, string type)
    {
        Rect = rect;
        Type = type;
    }
    public override string ToString()
    {
        return $"{(int)Rect.xMin},{(int)Rect.yMin},{(int)Rect.xMax},{(int)Rect.yMax},{Type}";
    }
}