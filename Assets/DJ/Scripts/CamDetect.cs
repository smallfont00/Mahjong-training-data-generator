using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CamDetect : MonoBehaviour
{
    public YoloData[] GetDetectedMjYoloData()
    {
        List<YoloData> data = new List<YoloData>();
        var mjList = GameObject.FindGameObjectsWithTag("MJ");
        foreach (GameObject mj in mjList)
        {
            if (Vector3.Dot((-mj.transform.up).normalized, (transform.position - mj.transform.position).normalized) < 0.3) continue;

            var boxSize = mj.GetComponent<Renderer>().bounds.extents;
            foreach (Transform child in mj.transform)
            {
                if (Physics.Raycast(transform.position, child.position - transform.position, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == mj)
                    {
                        var yoloData = hit.collider.gameObject.GetComponent<RandomStyle>().GetYoloData(GetComponent<Camera>());
                        data.Add(yoloData);
                        break;
                    }
                }
            }
        }
        return data.ToArray();
    }

    private void OnGUI()
    {
        if (GetComponent<Camera>() != Camera.main) return;
        var mjList = GameObject.FindGameObjectsWithTag("MJ");
        foreach (GameObject mj in mjList)
        {
            if (!mj.GetComponent<RandomStyle>().visiable) continue;
            if (Vector3.Dot((-mj.transform.up).normalized, (transform.position - mj.transform.position).normalized) < 0.3) continue;

            var boxSize = mj.GetComponent<Renderer>().bounds.extents;
            foreach (Transform child in mj.transform)
            {
                if (Physics.Raycast(transform.position, child.position - transform.position, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == mj)
                    {
                        var yoloData = hit.collider.gameObject.GetComponent<RandomStyle>().GetYoloData(GetComponent<Camera>());
                        GUI.Box(yoloData.Rect, yoloData.Type);
                        break;
                    }
                }
            }
        }
    }
}