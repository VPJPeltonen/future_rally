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
 
    public static string getTopNames(string selection){
        string tempstr = "";
        IOrderedEnumerable<KeyValuePair<string, float>> items;
        switch(selection){
            case "lap":
                items = from pair in LTscores
                            orderby pair.Value ascending
                            select pair;
                break;
            case "total":
                items = from pair in TTscores
                    orderby pair.Value ascending
                    select pair;
                break;
            default:
                items = from pair in TTscores
                    orderby pair.Value ascending
                    select pair;
                break;
        }
        foreach (KeyValuePair<string, float> pair in items)
        {
            tempstr += "\n"+ pair.Key;
        }
        return tempstr;
    }
    public static string getTopTimes(string selection){
        string tempstr = "";
        IOrderedEnumerable<KeyValuePair<string, float>> items;
        switch(selection){
            case "lap":
                items = from pair in LTscores
                            orderby pair.Value ascending
                            select pair;
                break;
            case "total":
                items = from pair in TTscores
                    orderby pair.Value ascending
                    select pair;
                break;
            default:
                items = from pair in TTscores
                    orderby pair.Value ascending
                    select pair;
                break;                
        }
        


        foreach (KeyValuePair<string, float> pair in items)
        {
            tempstr += "\n"+ pair.Value;
        }
        return tempstr;
    }
}
