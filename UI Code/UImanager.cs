﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public GameObject pausemenu;

    void Update()
    {
        if(Input.GetKeyUp("escape") ){
           if(Time.timeScale != 0){
                Time.timeScale = 0;
                pausemenu.gameObject.SetActive(true);
                Cursor.visible = true;
           }else{
                Time.timeScale = 1;
                pausemenu.gameObject.SetActive(false);
                Cursor.visible = false;
           }
        }
    }
    public void Continue(){
        Debug.Log("beeb");
        Time.timeScale = 1;
        pausemenu.gameObject.SetActive(false);            
    }

    public void Quit() => Application.Quit();
}
