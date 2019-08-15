using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    private float hoverForce = 50f;
    private float hoverHeight = 5f;
    protected Rigidbody rigidbody;
    protected Quaternion stableRotation;
    protected float xRotation,zRotation,yRotation;
    private float stabilizerPower = 500f;
    private bool on = true;

    public bool On { get => on; set => on = value; }

    void Awake()
    {
        rigidbody = GetComponent <Rigidbody>();
        //get original rotations for stabilisation
        zRotation =  transform.eulerAngles.z;
        xRotation = transform.eulerAngles.x;
        yRotation = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(On){
            Hover();
            Stabilize();
        }
    }

    //hovering code
    protected void Hover(){
        //try to find ground
        //Ray ray = new Ray (transform.position, -transform.up);
        Vector3 vektori = new Vector3(0,-1,0);
        Ray ray = new Ray (transform.position, vektori);
        RaycastHit hit;
        //if found ground push against it
        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce * 1.35f;
            rigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
    }

    protected void Stabilize(){
        //get the current y rotation so it wont push for it
        stableRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        //push the ship towards stability
        transform.rotation = Quaternion.Lerp(transform.rotation, stableRotation,  Time.time * stabilizerPower);
    }
}
