using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RacerScore
{
    public float time;
    public string racerName;  
    public string track;

    public RacerScore(string Name, float Time, string Track)
    {
        racerName = Name;
        time = Time;       
        track = Track;
    }

}
