using System;
using CommandLineTest;
using UnityEngine;

public class CreateMJ : MonoBehaviour
{
    [Serializable]
    public struct Flip { public bool X, Y, Z; }
    public GameObject ModelObject;
    public Margin margin;
    public MJCount mahjongCount;
    [CustomMin(0)]
    public float ModelScale = 1;
    public Flip RandomFlip;

    public void CreateObjects()
    {
        transform.RemoveAllChild();
        if (ModelObject == null)
        {
            Debug.LogError("Model Object hasn't set.");
        }
        else
        {
            if (ModelObject.GetComponent<Renderer>() is Renderer renderer)
            {
                var boundSize = renderer.bounds.size * ModelScale;
                var step = boundSize + margin;
                var size = Vector3.Scale(step, mahjongCount) - margin;
                Vector3 start = (boundSize - size) / 2;
                Quaternion origin = transform.localRotation;
                transform.localRotation = Quaternion.identity;
                for (var y = 0; y < mahjongCount.Y_Count; y++)
                {
                    for (var z = 0; z < mahjongCount.Z_Count; z++)
                    {
                        for (var x = 0; x < mahjongCount.X_Count; x++)
                        {
                            var new_pos = start + new Vector3(x * step.x, y * step.y, z * step.z) + transform.position;
                            var obj = Instantiate(ModelObject, new_pos, ModelObject.transform.rotation, transform);
                            obj.transform.localScale *= ModelScale;
                        }
                    }
                }
                transform.localRotation = origin;
            }
            else
            {
                Debug.LogError("Model Object doesn't have Renderer");
            }
        }
    }
}

[Serializable]
public class Margin
{
    public float XDistance, YDistance, ZDistance;
    public static implicit operator Vector3(Margin margin)
    {
        return new Vector3(margin.XDistance, margin.YDistance, margin.ZDistance);
    }
}
[Serializable]
public class MJCount
{
    [CustomMin(1)]
    public int X_Count, Y_Count, Z_Count;
    public static implicit operator Vector3(MJCount mjCount)
    {
        return new Vector3(mjCount.X_Count, mjCount.Y_Count, mjCount.Z_Count);
    }
}