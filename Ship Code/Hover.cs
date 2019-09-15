using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Hover : MonoBehaviour
{
    public Transform path;
    protected List<Transform> nodes;
    protected Transform[] checkPointList;
    //protected Transform Checkpoints;
    protected Timer timer;
    protected int currentNode = 0;
    protected List<Transform> racerList = new List<Transform>();
    protected Dictionary<Transform,float> racerDistances =  new Dictionary<Transform, float>();

    [Header("Engines Properties")]
    //engine effect
    public ParticleSystem thruster1, thruster2, turbo1,turbo2;
    //forward speed
    public float speed = 32f;
    //turning speed
    public float turnSpeed = 0.25f;
    //tilt speed of the ship
    protected float tiltSpeed = 0.2f;
    //force to push the ship off the ground
    protected float hoverForce = 200f;

    protected float hoverDamb = 1f;
    //height the hover tries to keep
    protected float hoverHeight = 5f;
    //power to keep ship up right
    protected float stabilizerPower = 0.1f;
    protected float angle = 0;
    //acceleration
    protected float[] accelerationRate = { 0.005f, 0.0025f, 0.0025f, 0.002f, 0.001f };//0.0025f;
    protected float[] gearValues = { 0, 1.4f, 2.1f, 2.6f, 3.1f };
    protected float[] revMod = { 1.5f, 0.8f, 0.6f, 0.5f, 0.5f };
    protected int shipToughness = 25;
    

    //is engine on
    [Header("Engine Controls")]
    protected bool engineOn = true;
    protected bool controlsActive = false;
    protected float powerInput,turnInput,xRotation,zRotation;
    protected float accel = 0f;
    protected float reverseAccel = 0f;
    //timer for how long engine is off after collission 
    protected int collissionTimer = 0;
    //ship resistance to collission
    
    protected Rigidbody shipRigidbody;
    protected Quaternion stableRotation;

    [Header("Stats")]
    protected int currentLaP = 0;
    protected int currentCP = 0;
    protected int currentSector = 0;
    protected int currentGear = 1;
    protected int maxGear = 5;


    [Header("Audio stuff")]
    protected AudioSource aSource;
    public AudioClip crashSound1,crashSound2;//,engineSounds,turboSounds;
    //public AudioSource crash, crash2;//engineSound, thrusterSound, turboSound, 
    private float jetPitch;
    private const float LowPitch = 0.2f;
    private const float HighPitch = 2f;
    private const float SpeedToRevs = 1.5f;

    [Header("UI stuff")]
    public RaceOrderKeeper keeper;
    public int carNumber;
    public string racerName = "Steve Noname";
    protected float moveSpeed;
    protected Vector3 lastPosition;

    [Header("Drafting Stuff")]
    protected float baseDrag = 1.25f;
    protected float draftRange = 80f;
    protected float draftRayX = 1f;//forward position
    protected float draftRayY = 0.01f;//ray height
    protected float draftRayAngle = 2f;
    private bool isDrafting = false;
    public bool isDragZone = false;
    protected string dragState = "normal";
    protected int counter,draftCounter = 0;

    [Header("Ram Stuff")]
    protected bool inRamRange = false;
    protected float RamRange = 10f;
    protected Transform ramTarget;
    protected float ramTargetDist;

    [Header("Effects")]
    public GameObject usedDust,desertDust,darkDesertDust,windEffects,bottomThruster;

    public ThrusterEngine shipsEngine;

    public bool IsDrafting { get => isDrafting; set => isDrafting = value; }

    private void Start(){
        aSource = GetComponent<AudioSource>();
        shipRigidbody = GetComponent <Rigidbody>();
        counter = 0;
    }

    protected void Awake () 
    {
        findTimer();
        findNodes();
        findTrack();
        sceneSetUp();
        lastPosition = transform.position;         
        //get original rotations for stabilisation
        zRotation =  transform.eulerAngles.z;
        xRotation = transform.eulerAngles.x;
    }

    public void endRace() => controlsActive = false;

    public void checkPoint(int CPnum){
        //check if last checkpoint has been passed 
        if (CPnum == currentCP+1){
            
            currentCP = CPnum;
            Debug.Log(currentCP);
        }
    }

    public float getPosition(){
        Transform target;
        if(currentCP==3){
            target = checkPointList[1];
        }else{
            target = checkPointList[(currentCP+2)*3];//the weird number is because the list of checkpoints has some unnecessary items
        }
        float distance = Vector3.Distance(target.position, transform.position); 
        float lapPenalty = (3-currentLaP)*20000;
        float checkpointPenalty = (5-currentCP)*5000;
        distance = distance + lapPenalty + checkpointPenalty;
        return distance;
    }    

    protected void HoverShip(){
        //get world down
        Vector3 vektori = new Vector3(0,-1,0);
        Vector3 hoverSensor = transform.position;
        hoverSensor.y += 0.2f;
        //try to find ground
        Ray ray = new Ray (hoverSensor, vektori);
        RaycastHit hit;
        //if found ground push against it
        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = ( Mathf.Sqrt(hoverHeight) -  Mathf.Sqrt(hit.distance)) / hoverHeight;         
            if(proportionalHeight > 0){
                float upSpeed = shipRigidbody.velocity.y;
                //force amount
                float appliedHoverForce = (proportionalHeight * hoverForce * 1.35f)-(hoverDamb*upSpeed);
                //apply force
                shipRigidbody.AddForce(Vector3.up * appliedHoverForce, ForceMode.Acceleration);
            }
        }
    }

    //acceleration and turning
    protected void EngineControl(){
        //forward power
        if (powerInput>0){
            var thrust = getThrust();
            shipRigidbody.AddRelativeForce(0f, 0f, thrust);
            enableThrusters(true);
        }else{
            enableThrusters(false);
        }
        
        if (powerInput<0){
            //reverse power
            shipRigidbody.AddRelativeForce(0f, 0f, (powerInput * speed * reverseAccel)/2);
        }
        //ship turning and tilting
        shipRigidbody.AddRelativeTorque(0f, turnInput * turnSpeed, -(turnInput * tiltSpeed));
        Stablizer();
    }
    
    protected void findNodes(){
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();     
        for (int i = 0; i < pathTransforms.Length; i++){
            if(pathTransforms[i] != path.transform){
                nodes.Add(pathTransforms[i]);    
            }
        }
    }

    protected void findTimer() => timer = GameObject.FindWithTag("Timekeeper").GetComponent<Timer>();

    protected void findTrack(){
        GameObject trackholder = GameObject.FindWithTag("Track"); 
        checkPointList = trackholder.GetComponentsInChildren<Transform>(); 
        Debug.Log(checkPointList.Length);
    }


    protected void engineNoise(){
        var gearmod = gearValues[currentGear-1]/2;
        float engineRevs = Mathf.Abs (accel+gearmod) * SpeedToRevs;
        shipsEngine.adjustNoise(gearmod,engineRevs);   
    }

    protected void shiftgear(string direction){
        switch(direction){
            case "up":
                if (currentGear<maxGear){
                    currentGear++;
                    accel = 0f;                    
                }
                break;
            case "down":
                if (currentGear>1){
                    currentGear--;
                    accel = 0.99f;
                }
                break;
        }
    }    

    protected float getThrust(){
        float tempThrust = (powerInput * speed * accel * revMod[currentGear-1])+(powerInput * speed * gearValues[currentGear-1]);
        return tempThrust;
    }

    protected void enableThrusters(bool select){
        var em = thruster1.emission;
        em.enabled = select;
        em = thruster2.emission;
        em.enabled = select;
    }

    protected void CheckDraft(){
        //float draftPower = 0.25f;
        IsDrafting = false;
        RaycastHit hit; 
        Vector3 forwardDir = transform.forward;
        Vector3 rightDir = transform.right;
        for(int i = 0; i < 4; i++ ){
            Vector3 sensorStartPos = transform.position;
            sensorStartPos.y += draftRayY;
            sensorStartPos += forwardDir * draftRayX;            
            sensorStartPos.y += draftRayY-0.3f+0.3f*i;
            //center sensor
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, draftRange)  && isShip(hit))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red);
                IsDrafting = true; 
            }     
            //right sensor
            sensorStartPos += 0.3f * rightDir;
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, draftRange) && isShip(hit))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red);
                IsDrafting = true; 
            }
            //right angle sensor
            if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(draftRayAngle, transform.up) * transform.forward, out hit, draftRange)  && isShip(hit))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red);
                IsDrafting = true;
            }
            //left sensor
            sensorStartPos += 0.6f * -rightDir;
            if (Physics.Raycast(sensorStartPos, transform.forward, out hit, draftRange) && isShip(hit))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red);
                IsDrafting = true; 
            }   
            //left angle sensor
            if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-draftRayAngle, transform.up) * transform.forward, out hit, draftRange) && isShip(hit))
            {
                Debug.DrawLine(sensorStartPos, hit.point, Color.red);
                IsDrafting = true;
            }         
        }    
    }

    protected bool isShip(RaycastHit hit){
        if (hit.collider.CompareTag("PlayerShip") || hit.collider.CompareTag("AIship")){
            return true;
        }else{
            return false;
        }
    }    

    protected void setDrag(){
        //check what state should be
        var tempState = dragState;
        if(IsDrafting){
            draftCounter = 45;
            if(isDragZone){
                dragState = "zone drafting";
            }else{
                dragState = "drafting";
            }
        }else{
            if(isDragZone){
                dragState = "zone";
            }else{
                if(draftCounter >= 1){
                    dragState = "drafting";
                }else{
                    dragState = "normal";
                }
            }
        }
        //see if state is different from old one
        if(tempState != dragState){
            switch(dragState){
                case "normal":
                    shipRigidbody.drag = baseDrag;
                    break;
                case "zone":
                    shipRigidbody.drag = 2f;
                    break;
                case "drafting":
                    shipRigidbody.drag = baseDrag - 0.25f;                    
                    break;
                case "zone drafting":
                    shipRigidbody.drag = 1.75f;                  
                    break;                                                            
                default:
                    shipRigidbody.drag = baseDrag;
                    break;
            }
        }      
        if(draftCounter >= 1){
            draftCounter--;
        }  
    }

    protected void makeDust(){
        //Ray ray = new Ray (transform.position, -transform.up);
        //get world down
        Vector3 vektori = new Vector3(0,-1,0);
        Vector3 hoverSensor = transform.position;
        hoverSensor.y += 0.2f;
        //try to find ground
        Ray ray = new Ray (hoverSensor, vektori);
        RaycastHit hit;
        //if found ground push against it
        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(usedDust, hit.point, Quaternion.identity);
        }
    }    

    protected void sceneSetUp(){
         // Create a temporary reference to the current scene.
         Scene currentScene = SceneManager.GetActiveScene ();
 
         // Retrieve the name of this scene.
         var usedScene = currentScene.buildIndex;
         switch(usedScene){
            case 1://track1
                usedDust = desertDust;
                break;
            case 2://desert2
                usedDust = darkDesertDust;
                break;
            case 3://great cliff
                usedDust = desertDust;
                break;
            default:
                usedDust = desertDust;
                break;
         }
    }

    protected void Stablizer(){ 
        Vector3 forwardDir = transform.forward;
        Vector3 vektori = new Vector3(0,-1,0);
        Vector3 hoverSensor = transform.position + (0.2f * forwardDir);
        RaycastHit hit;

        Ray frontRay = new Ray (hoverSensor, vektori);
        float frontDist = 0;
        if (Physics.Raycast(frontRay, out hit)){
            frontDist = hit.distance;
        }

        hoverSensor -= 0.4f * forwardDir; 
        Ray backRay = new Ray (hoverSensor, vektori);
        float backDist = 0;
        if (Physics.Raycast(backRay, out hit)){
            backDist = hit.distance;
        }

        //plus means forward tilt
        if (backDist < 8 && frontDist < 8){
            angle = (1 - (backDist/frontDist)) * 2000;//higer number mean front is closer
        }
        if (backDist > 10 && frontDist  > 10){
            angle = 0;
        }
        angle = Mathf.Clamp(angle, -6, 6);
        var tempX = xRotation + angle;   
        
        //get the current y rotation so it wont push for it
        stableRotation = Quaternion.Euler(tempX, transform.eulerAngles.y, zRotation);
        //push the ship towards stability
        transform.rotation = Quaternion.Lerp(transform.rotation, stableRotation,  stabilizerPower);
    }

    protected void findRacers(){
        GameObject[] Aiships = GameObject.FindGameObjectsWithTag("AIship");
        GameObject playersShip = GameObject.FindWithTag("PlayerShip"); 
        //player
        //racerList.Add(playersShip.GetComponentInChildren<Transform>());
        racerDistances.Add(playersShip.GetComponentInChildren<Transform>(),0f);
        //aiships
        foreach(GameObject v in Aiships){
            //racerList.Add(v.GetComponentInChildren<Transform>());
            racerDistances.Add(v.GetComponentInChildren<Transform>(),0f);
        } 
        racerList = new List<Transform>(racerDistances.Keys);
    }

    protected bool racersClose(){
        bool racerClose = false;
        for(int i = 0; i < racerDistances.Count; i++){
            float dist = Vector3.Distance(racerList[i].position, transform.position);
            racerDistances[racerList[i]] = dist; // racerList[i].getPosition();
            if (dist < 81 && dist != 0){//ship is within interaction distance and not self
                racerClose = true;
            }
        }
        return racerClose;
    }

    protected void sideSensors(){
        inRamRange = false;
        ramTargetDist = 81f;
        Vector3 forwardDir = transform.forward;
        Vector3 rightDir = transform.right;
        Vector3 upDir = transform.up;
        for(int i = 0; i < 4; i++ ){
            sensorArray(rightDir, -forwardDir,upDir,i);
            sensorArray(-rightDir, forwardDir,upDir,i);
        }         
    }

    protected void sensorArray(Vector3 facingDir,Vector3 rightDir, Vector3 upDir, int i = 0){
        Vector3 sensorStartPos = transform.position;
        sensorStartPos += facingDir * draftRayX; 
        sensorStartPos += upDir * 0.3f * i;            
        //center sensor
        sensorRay(sensorStartPos,facingDir,10f);
        //right sensor
        sensorStartPos += 0.3f * rightDir;
        sensorRay(sensorStartPos,facingDir,10f);
        //right sensor
        sensorStartPos += 0.3f * rightDir;
        sensorRay(sensorStartPos,facingDir,10f);
        //right angle sensor 1
        sensorRay(sensorStartPos, Quaternion.AngleAxis(5f, upDir) * facingDir, 10f);
        //right angle sensor 2
        sensorRay(sensorStartPos, Quaternion.AngleAxis(10f, upDir) * facingDir, 10f);
        //left sensor
        sensorStartPos += 0.9f * -rightDir;
        sensorRay(sensorStartPos,facingDir,10f);
        //left sensor
        sensorStartPos += 0.3f * -rightDir;
        sensorRay(sensorStartPos,facingDir,10f);
        //left angle sensor
        sensorRay(sensorStartPos,Quaternion.AngleAxis(-5f, upDir) * facingDir,10f);               
        //left angle sensor 2
        sensorRay(sensorStartPos,Quaternion.AngleAxis(-10f, upDir) * facingDir,10f);
    }

    protected void sensorRay(Vector3 start, Vector3 dir, float range){
        RaycastHit hit; 
        if (Physics.Raycast(start, dir, out hit, range)  ){//&& isShip(hit)){
            Debug.DrawLine(start, hit.point, Color.red);
            inRamRange = true; 
            if(hit.distance < ramTargetDist){
                ramTargetDist = hit.distance;
                ramTarget =  hit.collider.GetComponent<Transform>();
            }
        }  
    }

    protected void Ram(){
       /*  float ramForce = 10f;
        //get direction of target
        Vector3 heading = ramTarget.position - shipRigidbody.position;
        //apply force to that direction
        shipRigidbody.AddForce(heading * ramForce, ForceMode.Acceleration);
        shipsEngine.triggerRamThruster("left");*/
    }
}