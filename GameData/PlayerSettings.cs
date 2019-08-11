using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField]
    private Text playername, placeholder;
    public Toggle musicOn;

    public void Awake ()
    {
        checkName();
        checkMusic();
    }

    public void saveName(string name){
        PlayerPrefs.SetString("playerName", name);
        PlayerPrefs.Save ();
    }

    public void saveMusic(bool decision){
        int music = 0;
        if(decision)
            music = 1;
        PlayerPrefs.SetInt("music",music);
        GameController.setMusic(decision);
        PlayerPrefs.Save ();
    }

    private void checkName(){
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

    private void checkMusic(){
        if (PlayerPrefs.HasKey("music"))
        {
            int tempMusic = PlayerPrefs.GetInt("music");
            bool music = true;
            Debug.Log(tempMusic);
            if(tempMusic == 0){
                music = false;
                musicOn.isOn = false;
            }else{
                musicOn.isOn = true;
            }
            GameController.setMusic(music);
        }
        else
        {
            PlayerPrefs.SetInt("music", 1);
            PlayerPrefs.Save();
        }        
    }
}
