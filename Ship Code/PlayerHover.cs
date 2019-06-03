﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHover : Hover
{

    public GameObject hitScreen;
    public Text lapCounter;
    private void Start(){
        findTimer();
        findNodes();
        findTrack();
        lapCounter.text = "Lap \n 1/3";
    }

    void Update () 
    {
        if(engineOn){
            hitScreen.gameObject.SetActive(false);
            //get controls
            powerInput = Input.GetAxisRaw ("Vertical");
            turnInput = Input.GetAxis ("Horizontal");
            if(controlsActive){
                if(accel < 1 && powerInput>0){
                    accel += accelerationRate;
                }else{
                    if(accel > 0){
                        accel -= accelerationRate;
                    }
                }
                if(reverseAccel < 1 && powerInput<0){
                    reverseAccel += accelerationRate;
                }else{
                    if(reverseAccel > 0){
                        reverseAccel -= accelerationRate;
                    }
                }
            }
        }else{
            hitScreen.gameObject.SetActive(true);
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
        CheckWaypointDistance();
        engineNoise();
        //currentSector = getSector();
        if(engineOn){
            HoverShip();
            if(controlsActive){EngineControl();}
        }
    }
    //start controls 
    public void startRace() => controlsActive = true;

    private void CheckWaypointDistance(){
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 100f){ 
            if(currentNode == nodes.Count - 1){
                currentNode = 0;
            }else{
                currentNode++;                
            }
        }
    }
    
    private void playerEndRace(){
        controlsActive = false;
        timer.endRace();
    }

    public void finishLine(){
        if (currentCP == 3){
            timer.FinishLap();
            if(currentLaP == 2){
                playerEndRace();
            }else{
                currentCP = 0;
                currentLaP += 1;                
                lapCounter.text = "Lap\n" + (currentLaP+1)+"/3";
            }
        }
    }
}