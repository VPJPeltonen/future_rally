using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeUIController : MonoBehaviour
{
    public Toggle easyDif,normalDif,hardDif,normalMode,soloMode,ddMap,gcMap,acMap;

    // Start is called before the first frame update
    void Start()
    {
        checkMode();
        checkDifficulty();
        checkMap();
    }

    private void checkMode(){
        string mode = GameController.getMode();
        if(mode == "normal"){
            normalMode.isOn = true;
            soloMode.isOn = false;
        }else{
            soloMode.isOn = true;
            normalMode.isOn = false;
        }
    }
    private void checkDifficulty(){
        string dif = GameController.getDifficulty();
        Debug.Log(dif);
        switch(dif){
            case "easy":
                easyDif.isOn = true;
                normalDif.isOn = false;
                hardDif.isOn = false;
                break;
            case "normal":
                normalDif.isOn = true;
                easyDif.isOn = false;
                hardDif.isOn = false;
                break;
            case "hard":
                hardDif.isOn = true;
                easyDif.isOn = false;
                normalDif.isOn = false;                
                break;
            default:
                break;
        }
    }
    private void checkMap(){
        string map = GameController.getMap();
        acMap.isOn = false;
        ddMap.isOn = false;
        gcMap.isOn = false;
        switch(map){
            case "Ancient City":
                acMap.isOn = true;
                break;
            case "Deep Desert":
                ddMap.isOn = true;
                break;
            case "Great Cliff":
                gcMap.isOn = true;
                break;
            default:
                break;
        }
    }
}
