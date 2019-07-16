using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField]
    private Text playername, placeholder;

    public void Awake ()
    {
        if (PlayerPrefs.HasKey("playerName"))
        {
            string name = PlayerPrefs.GetString("playerName");
            playername.text = name;
            placeholder.text = name;
            GameController.setName2(name);
        }
        else
        {
            PlayerPrefs.SetString("playerName", playername.text);
            PlayerPrefs.Save();
        }
    }
    public void saveName(string name){
        PlayerPrefs.SetString("playerName", name);
        PlayerPrefs.Save ();
    }
}
