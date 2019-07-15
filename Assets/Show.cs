using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Show : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    private Renderer m_Renderer;

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        cam = Camera.current;
    }

    // Update is called once per frame
    void Update()
    {
        //DebugDrawColoredRectangle(boxPoint, boxSize, Color.blue);
       

    }

    void FixedUpdate()
    {
        var boxSize = m_Renderer.bounds.extents;
        //Debug.DrawLine(transform.position, transform.position - transform.up * boxSize.y * 0.5f);
        //Debug.DrawLine(transform.position, transform.position + transform.forward * boxSize.z);
    }
}
