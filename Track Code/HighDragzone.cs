using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighDragzone : MonoBehaviour
{
    public bool exit;
    void OnTriggerEnter(Collider other){
        var ship = other.gameObject.GetComponent<Rigidbody>();
        var isAI = other.gameObject.GetComponent<AIHover>();
        if (ship != null && isAI != null){
            if(exit){
                ship.drag = 1.25f;
            }else{
                ship.drag = 2f;
            }
        }
    }   
}
