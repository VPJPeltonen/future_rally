using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu, modeMenu, difMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void playGame(){
        mainMenu.gameObject.SetActive(false);
        modeMenu.gameObject.SetActive(true);
    }
    public void backToMain(){
        modeMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}
