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

    public static List<RacerScore> TTscores=  new List<RacerScore>(){
        new RacerScore("Rasputin",135f),
        new RacerScore("Steve Noname",150f),
        new RacerScore("Turbo",165f),
        new RacerScore("Max Speed",180f),
        new RacerScore("Momo",195f),
        new RacerScore("Rasputin",210f),
        new RacerScore("Bob",225f),
        new RacerScore("Totally Not A Dog",240f),
        new RacerScore("Turbo",265f),
        new RacerScore("Momo",280f)
    };
    public static List<RacerScore> LTscores=  new List<RacerScore>(){
        new RacerScore("Rasputin",45f),
        new RacerScore("Steve Noname",50f),
        new RacerScore("Turbo",55f),
        new RacerScore("Max Speed",60f),
        new RacerScore("Momo",65f),
        new RacerScore("Rasputin",70f),
        new RacerScore("Bob",75f),
        new RacerScore("Totally Not A Dog",80f),
        new RacerScore("Turbo",85f),
        new RacerScore("Momo",90f)
    };
/* 
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
    }*/
 
    public static string getTopNames(string selection){
        string tempstr = "";
        List<RacerScore> items = sortstuff(LTscores);
        foreach (RacerScore score in items)
        {
            tempstr += "\n"+ score.racerName;
        }
        return tempstr;
    }
    public static string getTopTimes(string selection){
        string tempstr = "";
        List<RacerScore> items = sortstuff(LTscores);
        foreach (RacerScore score in items)
        {
            tempstr += "\n"+ parseTime(score.time);
        }
        return tempstr;
    }/* 
    private static IOrderedEnumerable<KeyValuePair<string, float>> orderLists(string selection){
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
        return items;
    }
*/
    private static string parseTime(float time){
        string minutes = ((int) time/60).ToString();
        float secondsFloat = (float)time % 60;
        string seconds = secondsFloat.ToString("f2");
        if (secondsFloat <= 9){
            seconds = "0" + secondsFloat.ToString("f2");
        }
        string timerTime = minutes + ":" + seconds;  
        return timerTime;
    }
    private static List<RacerScore> sortstuff(List<RacerScore> list){
        list.Sort((s1, s2) => s1.time.CompareTo(s2.time));
        return list;
    }
}
