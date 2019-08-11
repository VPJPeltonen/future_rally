using System.Collections;
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
                closeMenu();
           }
        }
    }

    private void closeMenu(){
        Time.timeScale = 1;
        pausemenu.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    public void Continue() => closeMenu();

    public void Quit() => Application.Quit();


}
