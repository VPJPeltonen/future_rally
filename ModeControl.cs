using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeControl : MonoBehaviour
{
    public GameObject AIships,position;
    // Start is called before the first frame update
    void Start()
    {   
        Debug.Log(GameController.PlayerName);
        disable();
        difficulty();
    }
    protected void Awake () {
   
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void disable(){
        if(GameController.Mode == "solo"){
            AIships.SetActive(false);
            position.SetActive(false);
        }
    }
    private void difficulty(){
        if (GameController.Mode == "normal"){
            GameObject[] Aiships = GameObject.FindGameObjectsWithTag("AIship");
            List<AIHover> AIlist = new List<AIHover>();
            foreach(GameObject v in Aiships){
                AIlist.Add(v.GetComponentInChildren<AIHover>());
            }
            foreach(AIHover v in AIlist){
                //AIlist.Add(v.GetComponentInChildren<AIHover>());
                v.setDifficulty(GameController.Difficulty);
            }
        }
    }
}
