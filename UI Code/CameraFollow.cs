using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 defaultDistance = new Vector3(0f,0.8f,-1f);
    public float distanceDamp = 0.1f;
    public float rotationalDamp = 0.05f;
    private string state;
    private int counter = 0;
    Transform camT;
    public Vector3 velocity = Vector3.one;

    void Awake() => camT = transform;

    void FixedUpdate(){ 
        switch(state){
            case "normal":
                SmoothFollow(1000f,true);
                break;
            case "crashed":
                SmoothFollow(1f,false);
                break;
            case "starting":
                SmoothFollow(30f,false);
                counter++;
                if (counter >= 100){
                    state = "normal";
                    counter = 0;
                }
                break;
            default:
                SmoothFollow(1000f,true);
                break;
        }
    }
    
    void SmoothFollow(float max, bool roll){
        Vector3 toPos = target.position + (target.rotation * defaultDistance);
        Vector3 curPos = Vector3.SmoothDamp(camT.position, toPos, ref velocity, distanceDamp, max);
        
        camT.position = curPos;
        if(roll){
            camT.LookAt(target, target.up);
        }else{
            camT.LookAt(target);
        }
        
    }

    public void playerStatusUpdate(string status) => state = status;
}
