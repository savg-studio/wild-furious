using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ranking
{
    public string id;
    public string name;
    public float time;
    public string character;
    public string circuit;
    public long date;

    override
    public string ToString()
    {
        return JsonUtility.ToJson(this, true);
    }
}
