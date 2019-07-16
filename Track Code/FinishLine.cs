using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : TrackObject
{
    void OnTriggerEnter(Collider other){
        var ship = other.gameObject.GetComponent<PlayerHover>();
        if (ship != null){
            if(other.gameObject.tag == "PlayerShip"){
                ship.finishLine();
            }
        }
    }
}   
