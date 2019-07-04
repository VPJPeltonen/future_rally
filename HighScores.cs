using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class HighScores : MonoBehaviour
{
    /*
    TT = total times
    LT = Lap Times
     */
    public static List<string> TTracernames = new List<string>();
    public static List<string> LTracernames = new List<string>();
    public static List<float> TTtimes = new List<float>();
    public static List<float> LTtimes = new List<float>();

    public static Dictionary<string,float> TTscores=  new Dictionary<string, float>();
    public static Dictionary<string,float> LTscores=  new Dictionary<string, float>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayScores(){

    }

    private void orderscores(Dictionary<string,float> list){
        string tempstr = "";
        //int posCount = 1;
        //int Pos = 0;
        var items = from pair in list
                    orderby pair.Value ascending
                    select pair;

        // Display results.
        foreach (KeyValuePair<string, float> pair in items)
        {
            tempstr += "\n"+ pair.Key + ". " + pair.Value;
            //if (pair.Key == playername){
            //    Pos = posCount;
            //    pos = Pos;
            //}
            //posCount++;
        }
       // PosNum.text = postionText(Pos);
       /// RaceOrderText.text=tempstr;
    }
}
