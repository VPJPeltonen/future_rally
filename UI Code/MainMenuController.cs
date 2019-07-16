using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu, modeMenu, difMenu, timesMenu;
    
    public void playGame(){
        mainMenu.gameObject.SetActive(false);
        modeMenu.gameObject.SetActive(true);
        timesMenu.gameObject.SetActive(false);
    }
    public void backToMain(){
        modeMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        timesMenu.gameObject.SetActive(false);
    }
    public void topTimes(){
        modeMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        timesMenu.gameObject.SetActive(true);
    }

}
