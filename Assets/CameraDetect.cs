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
    //public float rotateIntense;

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
        var circle = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;
        var sphere = new Vector3(circle.x, Random.Range(.5f, .8f), circle.y);
        var nextPosition = sphere.normalized * Random.Range(12f, 13f);
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
        transform.LookAt(m_Manager.gameObject.transform);
        //transform.localEulerAngles =
        //    m_OriginalLocalRotation + Random.insideUnitSphere * Random.Range(0f, rotateIntense);
        if (randomSphereEnable) RandomPositionSphere();
    }

    public bool CheckMjIsVisable(GameObject mj)
    {
        var thisCamera = this.gameObject.GetComponent<Camera>();
        if (!mj.GetComponent<RandomStyle>().m_visiable) return false;
        if (Vector3.Dot((-mj.transform.up).normalized, (transform.position - mj.transform.position).normalized) < 0.175) return false;
        var screenPoint = thisCamera.WorldToScreenPoint(mj.transform.position);

        float delta = 20;

        if (screenPoint.x <= delta || screenPoint.x >= Screen.width - delta) return false;
        if (screenPoint.y <= delta || screenPoint.y >= Screen.height - delta) return false;

        RaycastHit hit;
        int num_hit = 0;
        foreach (Transform point in mj.transform.GetChild(0).transform)
        {
            if (Physics.Raycast(transform.position, point.position - transform.position, out hit))
                if (hit.collider.gameObject == mj)
                    num_hit++;
        }
        if (num_hit >= 7) return true;
        return false;
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
        return;
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