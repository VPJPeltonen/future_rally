using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHover : Hover
{
    //uistuff
    public GameObject hitScreen;
    public Text lapCounter,gearText,speedText;
    public Slider engineSlider, turboSlider;   
    //turbostuff
    private float maxTurbo = 100f;
    private float currentTurbo = 0f;
    private float turboRegen = 0.05f;
    private bool turboInput;
    private float turboPower = 40f;
    private void Start(){
        findTimer();
        findNodes();
        findTrack();
        //enableThrusters(false);
        //enableTurbo(false);
        lapCounter.text = "1/3";
        gearText.text = "1";
    }

    void Update () 
    {
        if(engineOn){
            hitScreen.gameObject.SetActive(false);
            //get controls
            powerInput = Input.GetAxisRaw ("Vertical");
            turnInput = Input.GetAxis ("Horizontal");
            turboInput = Input.GetButton("Jump"); 
            if(controlsActive){
                gearBox();
                turboBooster();
            }
        }else{
            hitScreen.gameObject.SetActive(true);
            collissionTimer++;
            accel = 0;
            currentGear = 1;
            if(collissionTimer >= 150){
                engineOn = true;
                collissionTimer = 0;
            }
        }
        engineSlider.value = accel;
    }

    void FixedUpdate()
    {
        calculateSpeed();
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
                lapCounter.text = (currentLaP+1)+"/3";
            }
        }
    }

    private void gearBox(){
        //if not paused
        if(Time.timeScale == 1){
            //check if driver is accelerating
            if(powerInput>0){
                if(accel < 1){
                    accel += accelerationRate;
                }else{
                    shiftgear("up");
                    showGear();
                }            
            }else{
                if(accel > 0){
                    accel -= accelerationRate*4;
                }else{
                    shiftgear("down");
                    showGear();
                }
            }
            //check if reversing
            if(reverseAccel < 1 && powerInput<0){
                reverseAccel += accelerationRate;
            }else{
                if(reverseAccel > 0){
                    reverseAccel -= accelerationRate;
                }
            }
        }
    }

    private void showGear(){
        gearText.text = currentGear.ToString();
    }

    private void calculateSpeed(){
        moveSpeed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
        speedText.text = (Mathf.Round(moveSpeed*100)).ToString()+" km/h";
    }

    private void turboBooster(){
        if(turboInput){
            if(currentTurbo > 0){
                shipRigidbody.AddRelativeForce(0f, 0f, turboPower);
                currentTurbo -= 0.5f;
                enableTurbo(true);                          
            }else{
                enableTurbo(false);          
            }
        }else{
            enableTurbo(false);
        }
        if (currentTurbo < maxTurbo){
            currentTurbo += turboRegen;
        } 
        turboSlider.value = currentTurbo;
    }

    private void enableTurbo(bool select){
        var em = turbo1.emission;
        em.enabled = select;
        em = turbo2.emission;
        em.enabled = select;
    }
}
