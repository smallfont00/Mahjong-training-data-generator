using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomStyle : MonoBehaviour
{
    public bool m_filped = false;
    public int m_rotated = 0;
    public bool m_visiable = true;

    private string m_Type;
    private Vector3 m_OriginalLocalEulerAngles;
    private Collider m_Collider;
    private Renderer m_Renderer;

    static string[] resourcesPath = new[] { "type1/", "type2/", "type3/", "type4/", "type5/", "type6/", "type7/" };

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_Collider = GetComponent<Collider>();
        m_OriginalLocalEulerAngles = transform.localEulerAngles;

        //ChangeStyleRandomly();
    }



    private string ToPhotoFormat(int number)
    {
        return "p(" + number + ")";
    }
    public void ChangeStyleRandomly(bool flipEnable = false, bool visibleEnable = false, bool rotateEnable = false)
    {
        if (flipEnable) m_filped = Random.Range(0, 3) == 0;
        if (visibleEnable) m_visiable = Random.Range(0, 3) <= 1;
        if (rotateEnable) m_rotated = Random.Range(0, 4);

        m_Renderer.enabled = m_visiable;
        m_Collider.enabled = m_visiable;

        transform.localEulerAngles = m_OriginalLocalEulerAngles + (m_filped ? new Vector3(0, 180, 0) : Vector3.zero);

        if (rotateEnable) transform.Rotate(Vector3.up, m_rotated * 90, Space.World);

        var mat = GetComponent<Renderer>().materials[0];

        int typeNumber = resourcesPath.Length;
        var randomType = Random.Range(0, typeNumber);
        var randomCard = Random.Range(0, 42);

        if (typeNumber == resourcesPath.Length - 1) {
            int[] idx = { 9, 34, 35, 36, 37, 38, 39, 40, 41};
            randomCard = idx[Random.Range(0, idx.Length)];
        }
        var path = resourcesPath[randomType] + ToPhotoFormat(randomCard);

        var nextTexture = Resources.Load<Texture2D>(path);
        for (int i = (randomType + 1) % typeNumber, j = 0; nextTexture == null; i = (i + 1) % typeNumber,j++)
        {
            path = resourcesPath[i] + ToPhotoFormat(randomCard);
            nextTexture = Resources.Load<Texture2D>(path);
        }
        mat.mainTexture = nextTexture;
        m_Type = randomCard.ToString();
    }
    
    public YoloData GetYoloData(Camera cam)
    {
        float left = Screen.width, right = 0, up = 0, down = Screen.height;
        foreach (Transform point_transform in transform)
        {
            if (!point_transform.gameObject.activeSelf) continue;
            var tmp = cam.WorldToScreenPoint(point_transform.position);
            left = Mathf.Min(left, tmp.x);
            right = Mathf.Max(right, tmp.x);
            up = Mathf.Max(up, tmp.y);
            down = Mathf.Min(down, tmp.y);
        }

        //var boxPoint = GetComponent<Renderer>().bounds.center;
        //var boxSize = GetComponent<Renderer>().bounds.extents;

        //var boxUp = Vector3.up * boxSize.y;
        //var boxRight = Vector3.right * boxSize.x;
        //var boxForward = Vector3.forward * boxSize.z;



        //Vector3[] box = new Vector3[3] {boxUp, boxRight, boxForward};
        //float left = Screen.width, right = 0, up = 0, down = Screen.height;
        //for (int i = 0; i < (1 << box.Length); i++)
        //{
        //    var tmp = boxPoint;
        //    for (int j = 0; j < box.Length; j++) tmp += (((i & (1 << j)) != 0) ? (box[j]) : (-box[j]));
        //    //Debug.DrawRay(boxPoint, (tmp - boxPoint).normalized);
        //    tmp = cam.WorldToScreenPoint(tmp);
        //    left = Mathf.Min(left, tmp.x);
        //    right = Mathf.Max(right, tmp.x);
        //    up = Mathf.Max(up, tmp.y);
        //    down = Mathf.Min(down, tmp.y);
        //}

        // var a = cam.ScreenToWorldPoint(new Vector3(left, up, cam.nearClipPlane * 2));
        // var b = cam.ScreenToWorldPoint(new Vector3(right, up, cam.nearClipPlane * 2));
        // var c = cam.ScreenToWorldPoint(new Vector3(left, down, cam.nearClipPlane * 2));
        left = Mathf.Max(0, left);
        right = Mathf.Min(Screen.width, right);
        up = Mathf.Min(Screen.height, up);
        down = Mathf.Max(0, down);

        return new YoloData(new Rect(left, Screen.height - up, right - left, up - down), m_Type);
    }
}

public class YoloData
{
    public Rect Rect { get; }

    public string Type { get; }


    public YoloData(Rect rect, string type)
    {
        this.Rect = rect;
        this.Type = type;
    }
}