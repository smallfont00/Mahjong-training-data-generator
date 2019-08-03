using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class Array : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inseparable = false;

    public float Forwarddistance = 1.0f;
    public float Rightdistance = 1.0f;
    public float Updistance = 1.0f;

    public uint ForwardNumber = 0;
    public uint RightNumber = 0;
    public uint UpNumber = 0;

    public GameObject TargetObject;

    public bool flipEnable = false;
    public bool visibleEnable = false;
    public bool numberRandomEnable = false;
    
    public int randomRangeMin = 1;
    public int randomRangeMax = 7;
    
    public void ChangeRightNumberRandomly()
    {
       // Debug.Log(randomRangeMin + " " + randomRangeMax);
        if (numberRandomEnable)
        {
            foreach (Array array in FindObjectsOfType<Array>())
            {
                if (array.numberRandomEnable)
                {
                    RightNumber = (uint)Random.Range(Math.Max(0, randomRangeMin),
                        Math.Max(0, Math.Max(array.randomRangeMin, randomRangeMax)));
                }
                array.CreateObjects();
            }
        }
    }
    
    public void ChangeAllMjStyleRandomly()
    {
        foreach (Transform transform1 in transform)
        {
            var randomStyle = transform1.gameObject.GetComponent<RandomStyle>();
            randomStyle.ChangeStyleRandomly(flipEnable, visibleEnable);
        }
    }
    
    public void CreateObjects()
    {
        if (TargetObject == null) { Debug.Log("Target Object haven't set"); return; }
        Renderer renderer = TargetObject.GetComponent<Renderer>();
        if (renderer == null) { Debug.Log("Target Object doesn't have Renderer"); return; }

//#if UNITY_EDITOR
//        foreach (Transform child in transform) UnityEditor.EditorApplication.delayCall += () => { DestroyImmediate(child.gameObject); };
//#else
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        //foreach (Transform child in transform) DestroyImmediate(child.gameObject);
//#endif

        var bounds = renderer.bounds;
        var forwardStep = inseparable ? bounds.size.z : Forwarddistance;
        var rightStep = inseparable ? bounds.size.x : Rightdistance;
        var upStep = inseparable ? bounds.size.y : Updistance;

        var basePos = transform.localPosition - transform.forward * (ForwardNumber - 1) / 2f * forwardStep - transform.right * (RightNumber-1) / 2f * rightStep - transform.up * (UpNumber -1) / 2f * upStep;
        //var basePos = transform.localPosition - renderer.bounds.size / 2;
        //Debug.Log(RightNumber);
        for (var i = 0; i < ForwardNumber; i++)
        {
            for (var j = 0; j < RightNumber; j++)
            {
                for (var k = 0; k < UpNumber; k++)
                {
                    var pos = basePos + i * forwardStep * transform.forward + j * rightStep * transform.right + k * upStep * transform.up;
                    Instantiate(TargetObject, pos, transform.rotation * TargetObject.transform.rotation, transform);
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Array))]
public class ArrayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
         Array myScript = (Array)target;
 
         if (GUILayout.Button("Button"))
         {
             myScript.CreateObjects();
         }
    }
 }
#endif