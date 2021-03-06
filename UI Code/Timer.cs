﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    //ui text locations for the lap timer, checkpoint times and lap time history
    public TextMeshProUGUI timerText, totaltimerText, checkpointText, bestCheckpoints, checkpointDifference, lapHistory;
    public Button backbutton;
    public PlayerHover player;
    public Countdown centerTextPlace;
    public RaceOrderKeeper orderkeeper;
    public GameObject background, finalscreen;
    public GameController game;
    public int maxLaps = 3;
    public string track;
    
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
    private float[] lastCheckpoints = new float[3];

    //array for fastest laps checkpoints
    private float[] topCheckpoints = new float[2];
    void Start()
    {  
        Cursor.visible = false;
        timerText.text = "00:00:00";
        totaltimerText.text = "00:00:00";
        checkpointText.text = "";
        lapHistory.text = "";   
        checkpointDifference.text = "";
        bestCheckpoints.text = "";
    }

    void Update()
    {
        if(on){
            totaltimerText.text = getTime(racestartTime);
            timerText.text =  getTime(startTime);
        }
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
        if (currentL != 0){
            float difF = checkpoints[currentCP] - lastCheckpoints[currentCP];
            string difI;
            if (difF >= 0){
                difI = "+";
            }else{
                difI = "";
            }
            difI += (difF).ToString("f2");
            checkpointDifference.text += difI + "\n";
        }
        currentCP++;
    }

    //when player finishes lap
    public void FinishLap(){
        //save laptime
        lapHistory.text += getTime(startTime) + "\n";
        var playerName = GameController.getName();
        var playerTime = Time.time - startTime; 

        HighScores.newLapTime(playerName,playerTime,track);
        laptimes[currentL] = Time.time - startTime;
        setOldTimes();
        currentCP = 0;
        checkpointText.text = "";
        startTime = Time.time;
        currentL++;
    }

    string getTime(float selectedTime){
        float currentTime = Time.time - selectedTime; 
        string minutes = ((int) currentTime/60).ToString();
        float secondsFloat = (float)currentTime % 60;
        string seconds = secondsFloat.ToString("f2");
        if (secondsFloat <= 9){
            seconds = "0" + secondsFloat.ToString("f2");
        }
        string timerTime = minutes + ":" + seconds;  
        return timerTime;
    }

    public void endRace(){
        //save total time
        var playerName = GameController.getName();
        var playerTime = Time.time - racestartTime; 
        HighScores.newTotalTime(playerName,playerTime,track);
        game.SaveGame();
        finishScreen();
    }

    public void setOldTimes(){
        string firstC = Mathf.Floor(checkpoints[0] / 60) + ":" + (checkpoints[0]%60).ToString("f2");
        string secondC = Mathf.Floor(checkpoints[1] / 60) + ":" + (checkpoints[1]%60).ToString("f2");
        string thirdC = Mathf.Floor(checkpoints[2] / 60) + ":" + (checkpoints[2]%60).ToString("f2");
        bestCheckpoints.text = firstC + "\n" + secondC + "\n" + thirdC;
        checkpointDifference.text = "";
        for(int i = 0; i < 3; i++){
            lastCheckpoints[i] = checkpoints[i];
        }
    }

    private void finishScreen(){
        on = false;
        Cursor.visible = true;
        finalscreen.gameObject.SetActive(true);
        orderkeeper.RaceOn = false;
        int pos = orderkeeper.pos;
        string finishText = "Finish! \n" + "You finished " + postionText(pos);
        centerTextPlace.finish(finishText);
    }
    private string postionText(int position){
        string posText = position.ToString();
        if (position == 1){
            posText += "st";
        }else if (position == 2){
            posText += "nd";
        }else if (position == 3){
            posText += "rd";
        }else{
            posText += "th";
        }
        return posText;
    }
}