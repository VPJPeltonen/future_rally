using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeControl : MonoBehaviour
{
    public GameObject itself;
    // Start is called before the first frame update
    void Start()
    {
        disable();
    }
    protected void Awake () {
   
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void disable(){
        if(GameController.Mode == "solo"){
            itself.SetActive(false);
        }
    }
}
