using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public GameObject pausemenu, mainUI, pos, timeDisplay;
    private bool UIvisible = true;

    private void Awake() {
        //for some reason timescale of map is set to 0 if you restart the map from menu
        Time.timeScale = 1;
    }

    void Update()
    {
        if(Input.GetKeyUp("escape") ){
           if(Time.timeScale != 0){
                Time.timeScale = 0;
                pausemenu.gameObject.SetActive(true);
                Cursor.visible = true;
                Debug.Log("closed");
           }else{
                closeMenu();
                Debug.Log("open");
           }
        }
        if(Input.GetButton("HideUI") ){
            if(UIvisible){
                mainUI.gameObject.SetActive(false);
                pos.gameObject.SetActive(false);
                timeDisplay.gameObject.SetActive(false);
                UIvisible = false;
            }else{
                mainUI.gameObject.SetActive(true);
                pos.gameObject.SetActive(true);
                timeDisplay.gameObject.SetActive(true);
                UIvisible = true;
            }
        }            
    }

    private void FixedUpdate() {
    
    }

    private void closeMenu(){
        Time.timeScale = 1;
        pausemenu.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    public void Continue() => closeMenu();

    public void Quit() => Application.Quit();


}
