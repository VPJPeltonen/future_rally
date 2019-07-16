using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{
    protected Timer timekeeper;
    // Start is called before the first frame update
    void Start() => findTimer();

    protected void findTimer(){
        GameObject timerholder = GameObject.FindWithTag("Timekeeper"); 
        timekeeper = timerholder.GetComponent<Timer>();
    }
}
