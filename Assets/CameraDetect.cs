using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[System.Serializable]
public class CameraDetect : MonoBehaviour
{
    public bool randomSphereEnable = false;
    // Start is called before the first frame update
    private Vector3 m_OriginalLocalPosition;
    private Vector3 m_OriginalLocalRotation;

    public float translateIntense;
    public float rotateIntense;

    public bool onGuiEnable = false;

    private Manager m_Manager;
    public void SetManager(Manager manager)
    {
        //Debug.Log("fk");
        m_Manager = manager;
    }
    void Start()
    {
        ResetOriginalData();
        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }

        //m_Manager = transform.parent?.gameObject.GetComponent<Manager>();
        //if (m_Manager == null) Debug.Log("Camera not attach to manager");
    }
    public void RandomPositionSphere()
    {
        var sphere = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(.3f, .75f), Random.Range(-1f, -0.5f));
        var nextPosition = sphere.normalized * Random.Range(10f, 15f);
        nextPosition.y = Mathf.Abs(nextPosition.y);
        transform.localPosition = nextPosition;
        transform.LookAt(m_Manager.gameObject.transform);
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
        if (randomSphereEnable) RandomPositionSphere();
    }

    public bool CheckMjIsVisable(GameObject mj)
    {
        var thisCamera = this.gameObject.GetComponent<Camera>();
        if (!mj.GetComponent<RandomStyle>().m_visiable) return false;
        if (Vector3.Dot((-mj.transform.up).normalized, (transform.position - mj.transform.position).normalized) < 0.175) return false;
        var screenPoint = thisCamera.WorldToScreenPoint(mj.transform.position);

        float delta = 3;

        if (screenPoint.x <= delta || screenPoint.x >= Screen.width - delta) return false;
        if (screenPoint.y <= delta || screenPoint.y >= Screen.height - delta) return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, mj.transform.GetChild(0).position - transform.position, out hit))
        {
            if (hit.collider.gameObject != mj)
            {
                if (Physics.Raycast(transform.position, mj.transform.GetChild(1).position - transform.position, out hit))
                {
                    if (hit.collider.gameObject != mj) return false;
                }
            }
        }

        return true;
    }

    public YoloData[] GetDetectedMjYoloData()
    {
        List<YoloData> yoloDaces = new List<YoloData>();
        var mjList = m_Manager.GetManagerMj();
        

        foreach (GameObject mj in mjList)
        {
            if (!CheckMjIsVisable(mj)) continue;
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

    private void OnGUI()
    {
        //return;
        if (!this.onGuiEnable) return;

        var mjList = m_Manager?.GetManagerMj();
        if (m_Manager == null) return;
        foreach (GameObject mj in mjList)
        {
            if (!CheckMjIsVisable(mj)) continue;
            var yoloData = mj.GetComponent<RandomStyle>().GetYoloData(this.GetComponent<Camera>());
            GUI.Box(yoloData.Rect, yoloData.Type);

            //if (mj.transform.parent?.parent?.gameObject.name == "1") Debug.Log("1");
            //else Debug.Log("2");
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
            //            GUI.Box(yoloData.Rect, yoloData.Type);
            //            break;
            //        }
            //    }
            //}
        }
    }
}