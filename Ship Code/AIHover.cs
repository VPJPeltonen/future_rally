﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHover : Hover
{
    
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

    void OnDrawGizmos(){
        shipRigidbody = GetComponent <Rigidbody>();
        CheckSensors();
    }

    void Update () 
    {      
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
       // currentSector = getSector();
       engineNoise();
        if(engineOn){
            HoverShip();
            if(controlsActive){
                CheckSensors();
                Fly();
                CheckWaypointDistance();
                if(!avoiding){SteerShip();}
            }
        }
    }

    //start controls 
    public void startRace(){
        controlsActive = true;
        accelerating = true;
    }

    private void SteerShip(){
        transform.LookAt(nodes[currentNode]);

        //var q = Quaternion.LookRotation(nodes[currentNode].position - transform.position);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, turnSpeed * Time.deltaTime);
        
        stableRotation = Quaternion.Euler(xRotation,transform.eulerAngles.y, zRotation);
        transform.rotation = Quaternion.Lerp(transform.rotation, stableRotation,  Time.time * stabilizerPower);
    
    }

    private void Fly(){
        //forward power
        if(accelerating){
            shipRigidbody.AddRelativeForce(0f, 0f, powerInput * speed * accel);
            if(accel < 1){
                accel += accelerationRate;
            }
        }else{
            if(accel > 0){
                accel -= accelerationRate;
            }
        }
    }

    private void CheckSensors(){
        Vector3 sensorStartPos = transform.position;
        sensorStartPos.y += sensorHeight;
        Vector3 forwardDir = transform.forward;
        sensorStartPos += forwardDir * sensorX;
        avoiding = false;
        accelerating = true;
        float avoidNum = 0f;
        RaycastHit hit;
        //center sensor
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            avoiding = true;
            avoidNum -= 0.4f;
            accelerating = false;
        }

        Vector3 rightDir = transform.right;
        //right sensor
        sensorStartPos += sidesensorPos * rightDir;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            avoiding = true;
            avoidNum -= 1f;
            accelerating = false;
        }

        //right angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(sensorAngle, transform.up) * transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            avoiding = true;
            avoidNum -= 0.5f;
        }

        //left sensor
        sensorStartPos += 2 * sidesensorPos * -rightDir;
        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
            avoiding = true;
            avoidNum += 1f;
            accelerating = false;
        }

        //left angle sensor
        if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-sensorAngle, transform.up) * transform.forward, out hit, sensorLength) && shouldAvoid(hit))
        {
            Debug.DrawLine(sensorStartPos, hit.point);
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
}