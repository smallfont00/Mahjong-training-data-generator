using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawYolo : MonoBehaviour
{
    [Range(0, 3)]
    public int PlayerId = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        var playerSee = transform.Find($"Player ({PlayerId + 1})");
        foreach (Transform child in playerSee)
        {
        }
    }
}
