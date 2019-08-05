using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresController : MonoBehaviour
{
    public Text names,times;
    public GameController game;
    private string mode,track;

    // Start is called before the first frame update
    void Start()
    {
        mode = "lap";
        track = "ancient city";
        game.LoadGame();
        string NamesString, TimesString;
        NamesString = HighScores.getTopNames(mode,track);
        TimesString = HighScores.getTopTimes(mode,track);
        Debug.Log(NamesString);
        names.text = NamesString;
        times.text = TimesString;
    }

    public void switchDisplay(string selection){
        mode = selection;
        updateDisplay();
    }

    public void switchTrack(string newtrack){
        track = newtrack;
        updateDisplay();
    }

    public void updateDisplay(){
        string NamesString, TimesString;
        NamesString = HighScores.getTopNames(mode,track);
        TimesString = HighScores.getTopTimes(mode,track);
        names.text = NamesString;
        times.text = TimesString;
    }
}
