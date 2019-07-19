using CommandLineTest;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawYolo : MonoBehaviour
{
    private int playerId = -1;
    public string[] ImageDirectories = new[] { "type1/", "type2/", "type3/", "type4/", "type5/", "type6/" };

    public int PlayerId
    {
        get
        {
            return playerId;
        }
        set
        {
            playerId = ((value % 4) + 4) % 4;
            transform.localEulerAngles = 90 * playerId * Vector3.up;
        }
    }

    public void Next()
    {
        PlayerId++;
        if (playerId == 0)
        {
            Reset_MJ_Set();
        }
    }
    public void Reset_MJ_Set()
    {
        string ImageDirectory = ImageDirectories[UnityEngine.Random.Range(0, ImageDirectories.Length)];
        var ImageFiles = Resources.LoadAll<Texture2D>(ImageDirectory);
        Array.Sort(ImageFiles, (a, b) =>
        {
            var num1 = int.Parse(a.name.Trim('p', '(', ')'));
            var num2 = int.Parse(b.name.Trim('p', '(', ')'));
            return num1.CompareTo(num2);
        });
        Debug.Log(ImageDirectory);
        List<int> cardIdSet = new List<int>(144);
        if (ImageFiles.Length > 34)
        {
            cardIdSet.AddRange(Enumerable.Range(34, ImageFiles.Length - 34).ToArray());

        }
        foreach (var i in Enumerable.Repeat(Enumerable.Range(0, Math.Min(ImageFiles.Length, 34)).ToArray(), 4).ToArray())
        {
            cardIdSet.AddRange(i);
        }
        var shuffleIdSet = cardIdSet.ToArray();
        shuffleIdSet.Shuffle();
        int id = 0;
        for (int i = 1; i <= 4 && id < shuffleIdSet.Length; i++)
        {
            Transform trans = transform.Find($"Player ({i})");
            foreach (Transform child in trans)
            {
                var mat = child.GetComponent<Renderer>().materials[0];
                mat.mainTexture = ImageFiles[shuffleIdSet[id]];
                child.GetComponent<RandomStyle>().type = ImageFiles[shuffleIdSet[id]].name.Trim('p', '(', ')');
                id++;
                if (id >= shuffleIdSet.Length) break;
            }
        }
    }
}
