using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public AudioSource music;
    private bool shakeOn;
    private Vector3 defaultDistance = new Vector3(0f,0.6f,-2f);
    private Vector3 firstPersonDistance = new Vector3(0f,0f,1f);
    private float powerInput,turnInput;
    public float distanceDamp = 0.1f;
    public float rotationalDamp = 0.05f;
    private string state;
    private string mode = "1st person";
    //counter variables
    private int counter = 0;
    private int distortCounter = 0;
    private bool distortOn = false;
    Transform camT;
    public Vector3 velocity = Vector3.one;

    void Awake() => camT = transform;

    //stuff for god camera
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
 
    public float minimumX = -360F;
    public float maximumX = 360F;
 
    public float minimumY = -60F;
    public float maximumY = 60F;
 
    float rotationY = 0F;

    Rigidbody cameraBody;
    public PlayerHover player;
    public GameObject UI;
    public Material material;
    private float stableX;

    public bool DistortOn { get => distortOn; set => distortOn = value; }
    public bool ShakeOn { get => shakeOn; set => shakeOn = value; }

    void Start(){
        cameraBody = GetComponent <Rigidbody>();
        //toggle music based on setting
        bool musicPlay = GameController.getMusicSetting();
        //Debug.Log(musicPlay);
        music.enabled = musicPlay;
        shakeOn = true;
        stableX = target.transform.eulerAngles.x;
    }

    void Update(){
        checkButtons();
        switch(mode){
            case "3rd person":
                //thirdPersonMode();
                break;
            case "1st person":
                firstPersonMode();
                break;
            default:
                thirdPersonMode();
                break;
        }
    }



    void FixedUpdate(){ 
        switch(mode){
            case "3rd person":
                thirdPersonMode();
                break;
            case "1st person":
                //firstPersonMode();
                break;
            default:
                thirdPersonMode();
                break;
        }
    }

    public void playerStatusUpdate(string status) => state = status;
    public void startRace() => music.Play(0);

    public void changeView(){
        if(state != "switching mode"){
            state = "switching mode";
            if(mode == "1st person"){
                mode = "3rd person";
            }else{
                mode = "1st person";
            }
        }
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (DistortOn){
            Graphics.Blit(src,dest,material);
        }else{
            Graphics.Blit(src,dest);
        }
    }   

    private void godMode(){
        powerInput = Input.GetAxisRaw ("Vertical");
        turnInput = Input.GetAxis ("Horizontal");
        
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
           
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
           
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
           
            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
        //ship turning and tilting
        cameraBody.AddRelativeForce(200f*turnInput, 0f, 200f* powerInput);
//        camT.AddRelativeTorque(0f, turnInput * 1f, -(turnInput * 1f));
    }

    void SmoothFollow(float max, bool roll, Vector3 cameraDistance){
        Vector3 toPos = target.position + (target.rotation * cameraDistance);
        toPos.y = ((toPos.y + toPos.y + camT.position.y)/3);
        Vector3 curPos = Vector3.SmoothDamp(camT.position, toPos, ref velocity, distanceDamp, max);
        
        camT.position = curPos;
        if(roll){
            camT.LookAt(target, target.up);
        }else{
            camT.LookAt(target);
        }
    }
    private void firstPersonView(float max, bool roll, Vector3 cameraDistance){
       //Vector3 toPos = target.position; //+ (target.rotation * cameraDistance);
        //toPos.y = ((toPos.y + toPos.y + camT.position.y)/3);
        //Vector3 curPos = Vector3.SmoothDamp(camT.position, toPos, ref velocity, distanceDamp, max);
        Vector3 newRotation;
        if(shakeOn){
            float xVector = Mathf.Clamp(target.transform.eulerAngles.x, stableX-0.5f, stableX+0.5f);
            newRotation = new Vector3(xVector, target.transform.eulerAngles.y, target.transform.eulerAngles.z);
        }else{
            newRotation = new Vector3(stableX , target.transform.eulerAngles.y, target.transform.eulerAngles.z);
        }
       // newRotation = new Vector3(camT.transform.eulerAngles.x , target.transform.eulerAngles.y, target.transform.eulerAngles.z);
        camT.transform.eulerAngles = newRotation;       
        camT.position = target.position;        
    }

    private void thirdPersonMode(){
        switch(state){
            case "normal":
                SmoothFollow(1000f,false,defaultDistance);
                break;
            case "crashed":
                SmoothFollow(1f,false,defaultDistance);
                break;
            case "starting":
                SmoothFollow(30f,false,defaultDistance);
                counter++;
                if (counter >= 150){
                    distortOn = false;
                    state = "normal";
                    counter = 0;
                }
                break;
            case "god":
                godMode();
                break;
            case "switching mode":
                SmoothFollow(3000f,false,defaultDistance);
                counter++;
                if (counter >= 50){
                    distortOn = false;
                    state = "normal";
                    counter = 0;
                }            
                break;
            default:
                SmoothFollow(1000f,false,defaultDistance);
                break;
        }
    }

    private void firstPersonMode(){
        switch(state){
            case "normal":
                firstPersonView(10000f,true,firstPersonDistance);
                break;
            case "crashed":
                firstPersonView(1f,true,firstPersonDistance);
                break;
            case "starting":
                firstPersonView(30f,true,firstPersonDistance);
                counter++;
                if (counter >= 150){
                    distortOn = false;
                    state = "normal";
                    counter = 0;
                }
                break;
            case "god":
                godMode();
                break;
            case "switching mode":
                firstPersonView(3000f,false,defaultDistance);
                counter++;
                if (counter >= 50){
                    distortOn = false;
                    state = "normal";
                    counter = 0;
                }            
                break;                
            default:
                firstPersonView(1000f,true,firstPersonDistance);
                break;
        }        
    }
    private void checkButtons(){
        bool BButton = Input.GetKeyDown(KeyCode.B);
        bool NButton = Input.GetKeyDown(KeyCode.N);
        if(BButton && NButton){
            if(state == "god"){
                state = "normal";
                player.godMode(false);
                UI.gameObject.SetActive(true);
            }else{
                state = "god";
                player.godMode(true);
                UI.gameObject.SetActive(false);
            }
        }
    }
}
