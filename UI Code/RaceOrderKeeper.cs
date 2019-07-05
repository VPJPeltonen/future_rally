using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RaceOrderKeeper : MonoBehaviour
{
    //ui
    public Text RaceOrderText,PosNum;
    
    private int racerAmount;
    public int pos;
    private string playername;
    private Dictionary<string,float> raceStatus=  new Dictionary<string, float>();
    private List<string> racernames = new List<string>();
    //checkpointstuff
    public Transform Checkpoints;
    //Racers
    [Header("Racers")]
    public Transform player;
    private List<Hover> racerList = new List<Hover>();
    private Transform[] checkPointList;
    private int delayCounter = 0; 
    private bool raceOn = true;

    public bool RaceOn { get => raceOn; set => raceOn = value; }

    void Start()
    {
        findRacers();
        checkPointList = Checkpoints.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RaceOn){
            if (delayCounter >= 10){
                getDistances();
                delayCounter = 0;
            } 
            delayCounter++;
            showOrder();
        }
    }

    private void getDistances(){
        for(int i = 0; i < racerAmount ;i++){
            raceStatus[racernames[i]] = racerList[i].getPosition();
        }
    }
    private void showOrder(){
        string tempstr = "";
        int posCount = 1;
        int Pos = 0;
        var items = from pair in raceStatus
                    orderby pair.Value ascending
                    select pair;

        // Display results.
        foreach (KeyValuePair<string, float> pair in items)
        {
            tempstr += "\n"+ posCount + ". " + pair.Key;
            if (pair.Key == playername){
                Pos = posCount;
                pos = Pos;
            }
            posCount++;
        }
        PosNum.text = postionText(Pos);
        RaceOrderText.text=tempstr;
    }

    private void findRacers(){
        GameObject[] Aiships = GameObject.FindGameObjectsWithTag("AIship");
        GameObject playersShip = GameObject.FindWithTag("PlayerShip"); 
        List<float> emptytimes = new List<float>();
        //player
        racerList.Add(playersShip.GetComponentInChildren<Hover>());
        //racernames.Add(playersShip.GetComponentInChildren<Hover>().racerName);
        //playername = playersShip.GetComponentInChildren<Hover>().racerName;
        racernames.Add(GameController.PlayerName);
        playername = GameController.PlayerName;
        emptytimes.Add(0f);
        //aiships
        if(GameController.Mode == "normal"){
            foreach(GameObject v in Aiships){
                racerList.Add(v.GetComponentInChildren<Hover>());
                emptytimes.Add(0f);
                racernames.Add(v.GetComponentInChildren<Hover>().racerName);
            }
        }
        racerAmount = racerList.Count();
        raceStatus = racernames.Zip(emptytimes, (k, v) => new { Key = k, Value = v })
                    .ToDictionary(x => x.Key, x => x.Value);        
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


 