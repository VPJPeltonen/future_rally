using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Hover : MonoBehaviour
{
    public Transform path;
    protected List<Transform> nodes;
    protected Transform[] checkPointList;
    //protected Transform Checkpoints;
    protected Timer timer;
    protected int currentNode = 0;

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
    public float hoverForce = 50f;
    //height the hover tries to keep
    protected float hoverHeight = 5f;
    //power to keep ship up right
    protected float stabilizerPower = 0.003f;
    //acceleration
    public float accelerationRate = 0.0025f;
    protected float[] gearValues = { 0, 1.4f, 2.1f, 2.6f, 2.95f };
    protected float[] revMod = { 1.5f, 0.8f, 0.6f, 0.4f, 0.2f };
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
    public AudioSource engineSound, thrusterSound, turboSound, crash, crash2;
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

    public bool IsDrafting { get => isDrafting; set => isDrafting = value; }

    private void Start(){
        findTimer();
        findNodes();
        findTrack();
    }

    protected void Awake () 
    {
        lastPosition = transform.position; 
        shipRigidbody = GetComponent <Rigidbody>();
        //get original rotations for stabilisation
        zRotation =  transform.eulerAngles.z;
        xRotation = transform.eulerAngles.x;
    }

    

    //hovering code
    protected void HoverShip(){
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
            shipRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
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

        //get the current y rotation so it wont push for it
        stableRotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, zRotation);
        //push the ship towards stability
        transform.rotation = Quaternion.Lerp(transform.rotation, stableRotation,  Time.time * stabilizerPower);
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
        engineSound.pitch = Mathf.Clamp (engineRevs, LowPitch+gearmod, HighPitch+gearmod);       
        thrusterSound.pitch = Mathf.Clamp (engineRevs, LowPitch+gearmod, HighPitch+gearmod);       
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
        for(int i = 0; i < 4; i++ ){
            Vector3 sensorStartPos = transform.position;
            sensorStartPos.y += draftRayY;
            Vector3 forwardDir = transform.forward;
            Vector3 rightDir = transform.right;
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
            if(isDragZone){
                dragState = "zone drafting";
            }else{
                dragState = "drafting";
            }
        }else{
            if(isDragZone){
                dragState = "zone";
            }else{
                dragState = "normal";
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
    }
}