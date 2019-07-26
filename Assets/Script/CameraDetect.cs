using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class CameraDetect : MonoBehaviour
{
    private Vector3 m_OriginalLocalPosition;
    private Vector3 m_OriginalLocalRotation;
    public bool DisableGUI = false;
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
            var mjt = mj.transform;
            if (Vector3.Dot((-mjt.up).normalized, (transform.position - mjt.position).normalized) < 0.3) continue;

            var boxSize = mj.GetComponent<Renderer>().bounds.extents;
            foreach (Transform child in mj.transform)
            {
                if (Physics.Raycast(transform.position, child.position - transform.position, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == mj)
                    {
                        var yoloData = hit.collider.gameObject.GetComponent<RandomStyle>().GetYoloData(GetComponent<Camera>());
                        yoloDaces.Add(yoloData);
                        break;
                    }
                }
            }
        }
        return yoloDaces.ToArray();
    }

    private void OnGUI()
    {
        if (DisableGUI) return;
        if (GetComponent<Camera>() != Camera.main) return;
        var mjList = GameObject.FindGameObjectsWithTag("MJ");
        foreach (GameObject mj in mjList)
        {
            if (!mj.GetComponent<RandomStyle>().visiable) continue;
            var mjt = mj.transform;
            if (Vector3.Dot((-mjt.up).normalized, (transform.position - mjt.position).normalized) < 0.3) continue;

            var boxSize = mj.GetComponent<Renderer>().bounds.extents;
            foreach (Transform child in mj.transform)
            {
                if (Physics.Raycast(transform.position, child.position - transform.position, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == mj)
                    {
                        var yoloData = hit.collider.gameObject.GetComponent<RandomStyle>()
                            .GetYoloData(GetComponent<Camera>());
                        GUI.Box(yoloData.Rect, yoloData.Type);
                        break;
                    }
                }
            }
        }
    }
}