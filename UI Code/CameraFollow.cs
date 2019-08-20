using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public AudioSource music;
    private Vector3 defaultDistance = new Vector3(0f,0.6f,-2f);
    private float powerInput,turnInput;
    public float distanceDamp = 0.1f;
    public float rotationalDamp = 0.05f;
    private string state;
    private int counter = 0;
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

    public bool DistortOn { get => distortOn; set => distortOn = value; }

    void Start(){
        cameraBody = GetComponent <Rigidbody>();
        //toggle music based on setting
        bool musicPlay = GameController.getMusicSetting();
        Debug.Log(musicPlay);
        music.enabled = musicPlay;
    }

    void Update(){
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

    void FixedUpdate(){ 
        switch(state){
            case "normal":
                SmoothFollow(1000f,true);
                break;
            case "crashed":
                SmoothFollow(1f,false);
                break;
            case "starting":
                SmoothFollow(30f,false);
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
            default:
                SmoothFollow(1000f,false);
                break;
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        if (DistortOn){
            Graphics.Blit(src,dest,material);
        }else{
            Graphics.Blit(src,dest);
        }
    }   
    void SmoothFollow(float max, bool roll){
        Vector3 toPos = target.position + (target.rotation * defaultDistance);
        Vector3 curPos = Vector3.SmoothDamp(camT.position, toPos, ref velocity, distanceDamp, max);
        
        camT.position = curPos;
        if(roll){
            camT.LookAt(target, target.up);
        }else{
            camT.LookAt(target);
        }
        
    }

    public void playerStatusUpdate(string status) => state = status;

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

    public void startRace(){
        music.Play(0);
    }
}
