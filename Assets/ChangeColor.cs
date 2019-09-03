using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{

    private Color m_color;

    public float deltaR = 0;
    public float deltaG = 0;
    public float deltaB = 0;

    public Renderer targetRenderer;

    private void Awake()
    {
        m_color = targetRenderer.material.color;
    }

    private void Start()
    {
        m_color = targetRenderer.material.color;
    }
    public void change()
    {
        var next_color = m_color + new Color(Random.Range(-deltaR, deltaR), Random.Range(-deltaG, deltaG), Random.Range(-deltaB, deltaB), 0);
        //Debug.Log(m_color);
        //Debug.Log(next_color);
        targetRenderer.material.color = next_color;

    }
}
