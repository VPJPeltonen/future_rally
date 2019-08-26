using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHover : Hover
{
    //private float baseDrag = 1.25f;
    public float maxSteer = 45f;
    public bool testing;

    [Header("Sensorstuff")]
    private float sensorLength = 50f;
    private float sidesensorPos = 0.3f;
    private float sensorAngle = 30f;

    private float sensorHeight = 0.01f;
    private float sensorX = 0.45f;
    private bool avoiding;
    private bool accelerating;
    private bool breaking;

    //draws sensors to help see how far they reach
    void OnDrawGizmos(){
        shipRigidbody = GetComponent <Rigidbody>();
        CheckSensors();
    }

    void Update () 
    {      
        engineNoise();
        if(engineOn){
            powerInput = 1;
        }else{
            collissionTimer++;
            accel = 0;
            if(collissionTimer >= 150){
                engineOn = true;
                collissionTimer = 0;
            }
        }   
    }

    void FixedUpdate()
    {
        if (counter == 5){
            counter = 0;
            makeDust();
        }
        CheckDraft();
        setDrag();      
        if(engineOn){
            HoverShip();
            if(controlsActive){
                CheckSensors();
                Fly();
                CheckWaypointDistance();
                if(!avoiding){SteerShip();}
            }
        }
        counter++;
    }

    //start controls 
    public void startRace(){
        controlsActive = true;
        accelerating = true;
    }

    private void SteerShip(){
        transform.LookAt(nodes[currentNode]);

        stableRotation = Quaternion.Euler(xRotation,transform.eulerAngles.y, zRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, stableRotation,  Time.time * stabilizerPower);
    
    }

    private void Fly(){
        //forward power
        if(accelerating){
            var thrust = getThrust();
            shipRigidbody.AddRelativeForce(0f, 0f, thrust);
            if(accel < 1){
                accel += accelerationRate;
            }else{
                shiftgear("up");
            }   
        }else{
            if(accel > 0 && Time.timeScale == 1){
                accel -= accelerationRate;
            }else{
                shiftgear("down");
            }
        }
        if(breaking){
            var thrust = getThrust();
            shipRigidbody.AddRelativeForce(0f, 0f, -thrust);            
        }
    }

    private void CheckSensors(){
        Vector3 sensorStartPos = transform.position;
        sensorStartPos.y += sensorHeight;
        Vector3 forwardDir = transform.forward;
        sensorStartPos += forwardDir * sensorX;
        
        avoiding = false;
        accelerating = true;
        breaking = false;
        float avoidNum = 0f;
        RaycastHit hit;
        //center sensor
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            //Debug.DrawLine(sensorStartPos, hit.point, Color.red);
            avoiding = true;
            avoidNum -= 0.4f;
            accelerating = false;
            breaking = true;
        }

        Vector3 rightDir = transform.right;
        //right sensor
        sensorStartPos += sidesensorPos * rightDir;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            //Debug.DrawLine(sensorStartPos, hit.point);
            avoiding = true;
            avoidNum -= 1f;
            accelerating = false;
        }

        //right angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(sensorAngle, transform.up) * transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            //Debug.DrawLine(sensorStartPos, hit.point);
            avoiding = true;
            avoidNum -= 0.5f;
        }

        //left sensor
        sensorStartPos += 2 * sidesensorPos * -rightDir;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            //Debug.DrawLine(sensorStartPos, hit.point);
            avoiding = true;
            avoidNum += 1f;
            accelerating = false;
        }

        //left angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-sensorAngle, transform.up) * transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            //Debug.DrawLine(sensorStartPos, hit.point);
            avoiding = true;
            avoidNum += 0.5f;
        }
        if (avoiding){
            shipRigidbody.AddRelativeTorque(0f, avoidNum * turnSpeed, -(avoidNum * tiltSpeed));
            stableRotation = Quaternion.Euler(xRotation,transform.eulerAngles.y, zRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, stableRotation,  Time.time * stabilizerPower);        
        }
    }
    private void CheckWaypointDistance(){
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 25f){ 
            if(currentNode == nodes.Count - 1){
                currentNode = 0;
                currentLaP++;
                currentCP = 0;
                Debug.Log(currentLaP);
            }else{
                currentNode++;                
            }
        }
    }

    //check if sensor hits something it should avoid
    private bool shouldAvoid(RaycastHit hit){
        if (hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("PlayerShip") || hit.collider.CompareTag("AIship")){
            return true;
        }else{
            return false;
        }
    }

    public void setDifficulty(string difficulty){
        //Debug.Log(difficulty);
        switch(difficulty){
            case "easy":
                speed -= 18f;
                break;
            case "normal":
                //speed += 5f;
                break;   
            case "hard":
                speed += 15f;
                break;            
        }
    }
    //disable ship if hit stuff
    private void OnCollisionEnter(Collision collision){
        if(collision.relativeVelocity.magnitude > shipToughness){
            engineOn = false;
            crash.Play(0);
        }
    }

    private void effectsShow(){
        if(moveSpeed >= 1){
            windEffects.gameObject.SetActive(true);
            bottomThruster.gameObject.SetActive(false);
        }else{
            windEffects.gameObject.SetActive(false);
            bottomThruster.gameObject.SetActive(true);
        }
    }
}