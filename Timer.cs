using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //ui text locations for the lap timer, checkpoint times and lap time history
    public Text timerText, checkpointText, lapHistory, checkpointDifference;
    public Button backbutton;
    public PlayerHover player;
    public Countdown centerTextPlace;
    public int maxLaps = 3;
    
    //timestamp for lap start and time amount for top lap time
    private float startTime, toplap, racestartTime;
    private bool on = false;

    //to keep track of checkpoint and lap
    private int currentCP = 0;
    private int currentL = 0;

    
    //array to store laptimes
    private float[] laptimes = new float[10];

    //checkpoints for current lap
    private float[] checkpoints = new float[3];

    //array to store last laps checkpoints
    private float[] lastCheckpoints = new float[2];

    //array for fastest laps checkpoints
    private float[] topCheckpoints = new float[2];
    void Start()
    {  
        Cursor.visible = false;
        timerText.text = "";
        checkpointText.text = "";
        lapHistory.text = "";   
    }

    void Update()
    {
        if(on){timerText.text = getTime(racestartTime) + "\n" + getTime(startTime);}
    }

    //race starts
    public void startRace(){
        startTime = Time.time;
        racestartTime = Time.time;
        on = true;
    }


    //when player passes a checkpoint
    public void CheckpointTime(){
        checkpointText.text += getTime(startTime) + "\n";
        checkpoints[currentCP] = Time.time - startTime;
        currentCP++;
    }

    //when player finishes lap
    public void FinishLap(){
        lapHistory.text += getTime(startTime) + "\n";
        laptimes[currentL] = Time.time - startTime;
        currentCP = 0;
        checkpointText.text = "";
        //if (currentL >= 1){
            startTime = Time.time;
        //}
        currentL++;

    }

    string getTime(float selectedTime){
        float currentTime = Time.time - selectedTime; 
        string minutes = ((int) currentTime/60).ToString();
        float secondsFloat = (float)currentTime % 60;
        string seconds = secondsFloat.ToString("f2");;
        if (secondsFloat <= 10){
            seconds = "0" + secondsFloat.ToString("f2");
        }
        string timerTime = minutes + ":" + seconds;  
        return timerTime;
    }

    public void endRace(){
        on = false;
        Cursor.visible = true;
        backbutton.gameObject.SetActive(true);
        centerTextPlace.finish();
    }
}