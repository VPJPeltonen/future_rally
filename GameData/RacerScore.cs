using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RacerScore
{
    public float time;
    public string racerName;  
    
    public RacerScore(string p1, float  p2)
    {
        racerName = p1;
        time = p2;       
    }

}
