using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text timerText;
    public Timer timer;
    public PlayerHover player;
    public Transform AIShips;
    public AudioClip one,two,three,go;

    private bool stop = false;
    private float startTime;
    private float volume = 0.7f;
    AudioSource audioSource;
    private float timelimit = 4f;
    private bool goplayed,oneplayed,twoplayed,threeplayed = false;
    AIHover[] AIlist;
    void Start()
    {
        startTime = Time.time;
        AIlist = AIShips.GetComponentsInChildren<AIHover>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(!stop){
            float currentTime = Time.time - startTime; 
            float timething = timelimit-currentTime;            
            int timenumber = (int)timething;
            string seconds = timenumber.ToString();
            switch (timenumber){
                case 3:
                    playCount(threeplayed,three);
                    threeplayed = true;                
                    break;
                case 2:
                    playCount(twoplayed,two);
                    twoplayed = true;                
                    break;
                case 1:
                    playCount(oneplayed,one);
                    oneplayed = true;                
                    break;
            }
            if (string.Compare(seconds, "0") == 0){                
                if(!goplayed){
                    startRace();
                }
            }else{
                timerText.text = seconds; 
            }
            if (timething <= 0f){
                stop=true;
                timerText.text = "";
            }
        }
    }
    public void finish(string finishText) => timerText.text = finishText;

    public void playCount(bool playednumber, AudioClip sound){
        if(!playednumber){
            audioSource.PlayOneShot(sound, volume);            
        }        
    }
    private void startRace(){
        audioSource.PlayOneShot(go, volume);
        goplayed = true;
        timer.startRace();
        player.startRace();
        for (int i = 0; i < AIlist.Length; i++){
            AIlist[i].startRace();
        }                    
        timerText.text = "GO!";
    }
}
