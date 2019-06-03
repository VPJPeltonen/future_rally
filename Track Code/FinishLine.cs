using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : TrackObject
{
    /*public Timer timekeeper;
    public Checkpoint checkpoint1;
    public Checkpoint checkpoint2;
    public Checkpoint checkpoint3;*/
    void OnTriggerEnter(Collider other){
        var ship = other.gameObject.GetComponent<PlayerHover>();
        if (ship != null){
            if(other.gameObject.tag == "PlayerShip"){
                ship.finishLine();
            }
        }
    }
}   
