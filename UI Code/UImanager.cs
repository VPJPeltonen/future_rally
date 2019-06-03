using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImanager : MonoBehaviour
{
    public GameObject pausemenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        Time.timeScale = 1;
        pausemenu.gameObject.SetActive(false);            
    }

    public void Quit(){
        Application.Quit();
    }
}
