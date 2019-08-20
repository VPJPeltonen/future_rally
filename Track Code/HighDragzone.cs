using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighDragzone : MonoBehaviour
{
    public bool exit;
    void OnTriggerEnter(Collider other){
        var AI = other.gameObject.GetComponent<AIHover>();
        if (AI != null){
            if(exit){
                AI.isDragZone = false;
            }else{
                AI.isDragZone = true;
            }
        }
    }   
}
