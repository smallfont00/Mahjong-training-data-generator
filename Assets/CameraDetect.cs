using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public class CameraDetect : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 m_OriginalLocalPosition;
    private Vector3 m_OriginalLocalRotation;

    public float translateIntense;
    public float rotateIntense;

    void Start()
    {
        ResetOriginalData();
    }

    public void ResetOriginalData()
    {
        m_OriginalLocalPosition = transform.localPosition;
        m_OriginalLocalRotation = transform.localEulerAngles;
    }

    public void RandomSlightlyTransform()
    {
        transform.localPosition =
            m_OriginalLocalPosition + Random.insideUnitSphere * Random.Range(0f, translateIntense);
        transform.localEulerAngles =
            m_OriginalLocalRotation + Random.insideUnitSphere * Random.Range(0f, rotateIntense);
    }

    public YoloData[] GetDetectedMjYoloData()
    {
        List<YoloData> yoloDaces = new List<YoloData>();
        var mjList = GameObject.FindGameObjectsWithTag("MJ");
        foreach (GameObject mj in mjList)
        {
            if (!mj.GetComponent<RandomStyle>().visiable) continue;
            if (Vector3.Dot((-mj.transform.up).normalized, (transform.position - mj.transform.position).normalized) <
                0.3) continue;
            var yoloData = mj.GetComponent<RandomStyle>().GetYoloData(this.GetComponent<Camera>());
            yoloDaces.Add(yoloData);

            //RaycastHit hit;
            //var boxSize = mj.GetComponent<Renderer>().bounds.extents;
            //foreach (Transform child in mj.transform)
            //{
            //    if (Physics.Raycast(transform.position, child.position - transform.position, out hit))
            //    {
            //        if (hit.collider.gameObject == mj)
            //        {
            //            var yoloData = hit.collider.gameObject.GetComponent<RandomStyle>()
            //                .GetYoloData(this.GetComponent<Camera>());
            //            yoloDaces.Add(yoloData);
            //            break;
            //        }
            //    }
            //}
        }

        return yoloDaces.ToArray();
    }

    //private void OnGUI()
    //{
    //    if (this.gameObject.GetComponent<Camera>() != Camera.main) return;
    //    var mjList = GameObject.FindGameObjectsWithTag("MJ");
    //    foreach (GameObject mj in mjList)
    //    {
    //        if (!mj.GetComponent<RandomStyle>().visiable) continue;
    //        if (Vector3.Dot((-mj.transform.up).normalized, (transform.position - mj.transform.position).normalized) <
    //            0.3) continue;

    //        RaycastHit hit;
    //        var boxSize = mj.GetComponent<Renderer>().bounds.extents;
    //        foreach (Transform child in mj.transform)
    //        {
    //            if (Physics.Raycast(transform.position, child.position - transform.position, out hit))
    //            {
    //                if (hit.collider.gameObject == mj)
    //                {
    //                    var yoloData = hit.collider.gameObject.GetComponent<RandomStyle>()
    //                        .GetYoloData(this.GetComponent<Camera>());
    //                    GUI.Box(yoloData.Rect, yoloData.Type);
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //}
}