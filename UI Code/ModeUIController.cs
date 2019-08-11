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
        Debug.Log(mode);
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
        easyDif.isOn = false;
        normalDif.isOn = false;
        hardDif.isOn = false;
        switch(dif){
            case "easy":
                easyDif.isOn = true;
                break;
            case "normal":
                normalDif.isOn = true;
                break;
            case "hard":
                hardDif.isOn = true;
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
