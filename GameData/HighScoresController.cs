using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresController : MonoBehaviour
{
    public Text names,times;
    // Start is called before the first frame update
    void Start()
    {
        string NamesString, TimesString;
        NamesString = HighScores.getTopNames("lap");
        TimesString = HighScores.getTopTimes("lap");
        names.text = NamesString;
        times.text = TimesString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
