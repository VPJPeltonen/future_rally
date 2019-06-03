using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : TrackObject
{
    
    //public bool activated = false;
    public int number;
    void OnTriggerEnter(Collider other){
        var ship = other.gameObject.GetComponent<Hover>();
        if (ship != null){
            ship.checkPoint(number);
            if(other.gameObject.tag == "PlayerShip"){
                if (timekeeper != null){
                    timekeeper.CheckpointTime();
                }            
            }
        }
    }   
    
}
