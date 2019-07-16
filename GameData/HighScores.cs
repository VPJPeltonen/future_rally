using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HighScores : MonoBehaviour
{
    //TT = total times
    //LT = Lap Times
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
        new RacerScore("Rasputin",70f),
        new RacerScore("Max Speed",60f),
        new RacerScore("Momo",65f),
        new RacerScore("Bob",75f),
        new RacerScore("Totally Not A Dog",80f),
        new RacerScore("Turbo",85f),
        new RacerScore("Momo",90f)
    };

    public static string getTopNames(string selection){
        string tempstr = "";
        List<RacerScore> items = getList(selection);
        foreach (RacerScore score in items)
        {
            tempstr += "\n"+ score.racerName;
        }
        return tempstr;
    }
    public static string getTopTimes(string selection){
        string tempstr = "";
        List<RacerScore> items = getList(selection);
        foreach (RacerScore score in items)
        {
            tempstr += "\n"+ parseTime(score.time);
        }
        return tempstr;
    } 

    public static List<RacerScore> getTopLaptimes(){
        return LTscores;
    }
    public static void setToptimes(List<RacerScore> laptimes, List<RacerScore> totaltimes){
        LTscores = laptimes;
        TTscores = totaltimes;
    }

    public static List<RacerScore> getTopTotaltimes(){
        return TTscores;
    }

    private static List<RacerScore> getList(string selection){
        List<RacerScore> items;
        switch(selection){
            case "lap":
                items = sortstuff(LTscores);
                break;
            case "total":
                items = sortstuff(TTscores);
                break;
            default:
                items = sortstuff(LTscores);
                break;                
        }
        return items;
    }

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

    public static void newLapTime(string racerName,float time){
        RacerScore newScore = new RacerScore(racerName,time); 
        //add new time to list
        LTscores.Add(newScore);
        //drop lowest
        LTscores = sortstuff(LTscores);
        LTscores.RemoveAt(9);
    }
    public static void newTotalTime(string racerName, float time){
        RacerScore newScore = new RacerScore(racerName,time); 
        //add new time to list
        TTscores.Add(newScore);
        //drop lowest
        TTscores = sortstuff(TTscores);
        TTscores.RemoveAt(9);
    }
}
