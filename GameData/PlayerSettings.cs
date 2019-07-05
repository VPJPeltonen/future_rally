using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField]
    private Text playername, placeholder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Awake ()
    {
        if (!PlayerPrefs.HasKey("playerName"))
        {
            PlayerPrefs.SetString("playerName", playername.text);
            PlayerPrefs.Save ();
        }else{
            string name = PlayerPrefs.GetString ("playerName");
            Debug.Log(name);
            playername.text = name;
            placeholder.text = name;
            GameController.setName2(name);
        }
    }
    public void saveName(string name){
        PlayerPrefs.SetString("playerName", name);
        PlayerPrefs.Save ();
    }
}
