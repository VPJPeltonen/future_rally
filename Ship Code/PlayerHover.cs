using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHover : Hover
{
    //uistuff
    public TextMeshProUGUI lapCounter,gearText,speedText,debugText;
    public Slider engineSlider, turboSlider;   
    public CameraFollow camera;
    //turbostuff
    private float maxTurbo = 100f;
    private float currentTurbo = 0f;
    private float turboRegen = 0.05f;
    private bool turboInput,ramInput, UIshowInput,playing;
    private float turboPower = 40f;
    private int collissionImmunity = 0;
    private bool godmode = false;
    private bool debugMode = true;
    //effect things
    public GameObject draftEffects;

    private void Awake(){
        findTimer();
        findNodes();
        findTrack();
        findRacers();
        sceneSetUp();
        lastPosition = transform.position;         
        //get original rotations for stabilisation
        zRotation =  transform.eulerAngles.z;
        xRotation = transform.eulerAngles.x;
        lapCounter.text = "1/3";
        gearText.text = "1";
        baseDrag = 0.7f;
    }

    void Update (){  
        getInput();           
        engineNoise();
        if(debugMode){
            string debugInfo = "";
            if(inRamRange){
                debugInfo = "true";
            }else{
                debugInfo = "false";
            }
            debugText.text = debugInfo;
        }
        engineSlider.value = accel;
    }

    void FixedUpdate()
    {
        getInput();
        CheckWaypointDistance();
        if(engineOn){   
            if(controlsActive){
                
                gearBox();
                if(Time.timeScale == 1){
                    turboBooster();
                    if(ramInput && inRamRange){
                        Ram();
                    }
                }
            }
        }else{
            collissionTimer++;
            accel = 0;
            currentGear = 1;
            if(collissionTimer >= 150){
                engineOn = true;
                camera.playerStatusUpdate("starting");
                collissionTimer = 0;
            }
        }
        calculateSpeed();
        if(racersClose()){
            CheckDraft();
            sideSensors();
        }
        if(counter%5 == 0){
            effectsShow(); 
        }
        if(counter == 20){
            makeDust();
            counter = 0;
        }
        counter++;
        setDrag();
        if (collissionImmunity > 0){
            collissionImmunity--;
        }
        if(engineOn){
            HoverShip();
            if(controlsActive){
                EngineControl();
            }
        }
    }

    //start controls 
    public void startRace() => controlsActive = true;

    public void godMode(bool status){
        godmode = status;
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
                    accel += accelerationRate[currentGear-1];
                }else{
                    shiftgear("up");
                    showGear();
                }            
            }else{
                if(accel > 0){
                    accel -= accelerationRate[currentGear-1]*4;
                }else{
                    shiftgear("down");
                    showGear();
                }
            }
            //check if reversing
            if(reverseAccel < 1 && powerInput<0){
                reverseAccel += accelerationRate[currentGear-1];
            }else{
                if(reverseAccel > 0){
                    reverseAccel -= accelerationRate[currentGear-1];
                }
            }
        }
    }

    private void showGear() => gearText.text = currentGear.ToString();

    private void calculateSpeed(){
        moveSpeed = (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
        speedText.text = (Mathf.Round(moveSpeed*200)).ToString();
    }

    private void turboBooster(){
        if(turboInput){
            if(currentTurbo > 0){
                shipRigidbody.AddRelativeForce(0f, 0f, turboPower);
                currentTurbo -= 0.5f;
                if (!playing){
                    
//                    turboSound.Play(0);
                    playing = true;
                }
                shipsEngine.enableTurbo(true);                          
            }else{
                shipsEngine.enableTurbo(false);       
   //             turboSound.Stop();   
                playing = false;
            }
        }else{
            shipsEngine.enableTurbo(false);
//            turboSound.Stop();   
            playing = false;
        }
        if (currentTurbo < maxTurbo){
            currentTurbo += turboRegen;
        } 
        turboSlider.value = currentTurbo;
    }

  /*   private void enableTurbo(bool select){
        var em = turbo1.emission;

        //turboSound.Play(0);
        em.enabled = select;
        em = turbo2.emission;
        em.enabled = select;
    }
*/
    //disable ship if hit stuff
    private void OnCollisionEnter(Collision collision){
        //aSource.PlayOneShot(crashSound1, 0.7F);
        //Debug.Log(collision.relativeVelocity.magnitude);
        float noise = Random.Range(-1f, 1f);
        if(noise > 0){ //&& !crash.isPlaying){
            //crash.Play(0);
            aSource.PlayOneShot(crashSound1, 1F);
        }else{
            aSource.PlayOneShot(crashSound2, 1F);
            //crash2.Play(0);
            //if(!crash2.isPlaying){crash2.Play(0);}
        }
        
        if(collision.relativeVelocity.magnitude > shipToughness && collision.gameObject.tag != "AIship" && collision.gameObject.tag != "LightObject"){
            if(collissionImmunity < 1){
                collissionImmunity = 240;
                engineOn = false;
                camera.playerStatusUpdate("crashed");
                camera.DistortOn = true;
            }
        }
    }

    private void getInput(){
        bool cameraChangeInput = Input.GetButton("Camera");
        if(cameraChangeInput){
            camera.changeView();
        }
        UIshowInput = Input.GetButton("HideUI");
        if(!godmode && controlsActive && engineOn){
            powerInput = Input.GetAxisRaw ("Vertical");
            turnInput = Input.GetAxis ("Horizontal");
            turboInput = Input.GetButton("Turbo Boost"); 
            ramInput = Input.GetButton("Ram");
            
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
        if(dragState == "drafting" && moveSpeed >= 1){ 
            draftEffects.gameObject.SetActive(true);
        }else{
            draftEffects.gameObject.SetActive(false);
        }
    }


}
