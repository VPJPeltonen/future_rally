using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 defaultDistance = new Vector3(0f,0.8f,-1f);
    public float distanceDamp = 0.1f;
    public float rotationalDamp = 0.05f;
    
    Transform camT;
    public Vector3 velocity = Vector3.one;

    void Awake() => camT = transform;

    void FixedUpdate() => SmoothFollow();

    void SmoothFollow(){
        Vector3 toPos = target.position + (target.rotation * defaultDistance);
        Vector3 curPos = Vector3.SmoothDamp(camT.position, toPos, ref velocity, distanceDamp);
        camT.position = curPos;

        camT.LookAt(target, target.up);
    }

}
