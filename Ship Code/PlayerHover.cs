using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHover : Hover
{
    //uistuff
    public GameObject hitScreen;
    public TextMeshProUGUI lapCounter,gearText,speedText;
    public Slider engineSlider, turboSlider;   
    public CameraFollow camera;
    //turbostuff
    private float maxTurbo = 100f;
    private float currentTurbo = 0f;
    private float turboRegen = 0.05f;
    private bool turboInput,playing;
    private float turboPower = 40f;
    private int collissionImmunity = 0;
    private void Start(){
        findTimer();
        findNodes();
        findTrack();
        lapCounter.text = "1/3";
        gearText.text = "1";
    }

    void Update () 
    {
        if(engineOn){
            hitScreen.gameObject.SetActive(false);     
            if(controlsActive){
                getInput();
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
                camera.playerStatusUpdate("starting");
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
        if (collissionImmunity > 0){
            collissionImmunity--;
        }
        if(engineOn){
            HoverShip();
            if(controlsActive){EngineControl();}
        }
    }


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

    private void showGear() => gearText.text = currentGear.ToString();

    private void calculateSpeed(){
        moveSpeed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
        speedText.text = (Mathf.Round(moveSpeed*100)).ToString();
    }

    private void turboBooster(){
        if(turboInput){
            if(currentTurbo > 0){
                shipRigidbody.AddRelativeForce(0f, 0f, turboPower);
                currentTurbo -= 0.5f;
                if (!playing){
                    turboSound.Play(0);
                    playing = true;
                }
                enableTurbo(true);                          
            }else{
                enableTurbo(false);       
                turboSound.Stop();   
                playing = false;
            }
        }else{
            enableTurbo(false);
            turboSound.Stop();   
            playing = false;
        }
        if (currentTurbo < maxTurbo){
            currentTurbo += turboRegen;
        } 
        turboSlider.value = currentTurbo;
    }

    private void enableTurbo(bool select){
        var em = turbo1.emission;
        //turboSound.Play(0);
        em.enabled = select;
        em = turbo2.emission;
        em.enabled = select;
    }
    //disable ship if hit stuff
    private void OnCollisionEnter(Collision collision){
        float noise = Random.Range(-1f, 1f);
        if(noise > 0){
            crash.Play(0);
        }else{
            crash2.Play(0);
        }
        if(collissionImmunity < 1){
            collissionImmunity = 240;
            engineOn = false;
            camera.playerStatusUpdate("crashed");
        }
    }

    private void getInput(){
        powerInput = Input.GetAxisRaw ("Vertical");
        turnInput = Input.GetAxis ("Horizontal");
        turboInput = Input.GetButton("Jump"); 
    }

    //start controls 
    public void startRace() => controlsActive = true;

    public void godMode(bool status){
        controlsActive = status;
    }
}
