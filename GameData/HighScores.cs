using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighScores : MonoBehaviour
{
    //TT = total times
    //LT = Lap Times
    public static List<RacerScore> TTscores =  new List<RacerScore>(){
        new RacerScore("Rasputin",135f,"ancient city"),
        new RacerScore("Steve Noname",150f,"ancient city"),
        new RacerScore("Turbo",165f,"ancient city"),
        new RacerScore("Max Speed",180f,"ancient city"),
        new RacerScore("Momo",195f,"ancient city"),
        new RacerScore("Rasputin",210f,"ancient city"),
        new RacerScore("Bob",225f,"ancient city"),
        new RacerScore("Totally Not A Dog",240f,"ancient city"),
        new RacerScore("Turbo",265f,"ancient city"),
        new RacerScore("Momo",280f,"ancient city"),
        
        new RacerScore("Rasputin",195f,"deep desert"),
        new RacerScore("Steve Noname",210f,"deep desert"),
        new RacerScore("Turbo",225f,"deep desert"),
        new RacerScore("Max Speed",240f,"deep desert"),
        new RacerScore("Momo",255f,"deep desert"),
        new RacerScore("Rasputin",270f,"deep desert"),
        new RacerScore("Bob",285f,"deep desert"),
        new RacerScore("Totally Not A Dog",300f,"deep desert"),
        new RacerScore("Turbo",315f,"deep desert"),
        new RacerScore("Momo",330f,"deep desert"),

        new RacerScore("Rasputin",225f,"great cliff"),
        new RacerScore("Steve Noname",240f,"great cliff"),
        new RacerScore("Turbo",255f,"great cliff"),
        new RacerScore("Max Speed",270f,"great cliff"),
        new RacerScore("Momo",285f,"great cliff"),
        new RacerScore("Rasputin",300f,"great cliff"),
        new RacerScore("Bob",315f,"great cliff"),
        new RacerScore("Totally Not A Dog",330f,"great cliff"),
        new RacerScore("Turbo",345f,"great cliff"),
        new RacerScore("Momo",360f,"great cliff")        
    };
    public static List<RacerScore> LTscores =  new List<RacerScore>(){
        new RacerScore("Rasputin",45f,"ancient city"),
        new RacerScore("Steve Noname",50f,"ancient city"),
        new RacerScore("Turbo",55f,"ancient city"),
        new RacerScore("Rasputin",70f,"ancient city"),
        new RacerScore("Max Speed",60f,"ancient city"),
        new RacerScore("Momo",65f,"ancient city"),
        new RacerScore("Bob",75f,"ancient city"),
        new RacerScore("Totally Not A Dog",80f,"ancient city"),
        new RacerScore("Turbo",85f,"ancient city"),
        new RacerScore("Momo",90f,"ancient city"),

        new RacerScore("Rasputin",65f,"deep desert"),
        new RacerScore("Steve Noname",70f,"deep desert"),
        new RacerScore("Turbo",75f,"deep desert"),
        new RacerScore("Rasputin",80f,"deep desert"),
        new RacerScore("Max Speed",85f,"deep desert"),
        new RacerScore("Momo",90f,"deep desert"),
        new RacerScore("Bob",95f,"deep desert"),
        new RacerScore("Totally Not A Dog",100f,"deep desert"),
        new RacerScore("Turbo",105f,"deep desert"),
        new RacerScore("Momo",110f,"deep desert"),        

        new RacerScore("Rasputin",75f,"great cliff"),
        new RacerScore("Steve Noname",80f,"great cliff"),
        new RacerScore("Turbo",85f,"great cliff"),
        new RacerScore("Rasputin",90f,"great cliff"),
        new RacerScore("Max Speed",95f,"great cliff"),
        new RacerScore("Momo",100f,"great cliff"),
        new RacerScore("Bob",105f,"great cliff"),
        new RacerScore("Totally Not A Dog",110f,"great cliff"),
        new RacerScore("Turbo",115f,"great cliff"),
        new RacerScore("Momo",120f,"great cliff")            
    };

    public static string getTopNames(string nameSelect, string trackSelect){
        string tempstr = "";
        List<RacerScore> items = getList(nameSelect,trackSelect);
        foreach (RacerScore score in items)
        { 
            tempstr += score.racerName + "\n";
        }
        return tempstr;
    }
    public static string getTopTimes(string nameSelect, string trackSelect){
        string tempstr = "";
        List<RacerScore> items = getList(nameSelect,trackSelect);
        foreach (RacerScore score in items)
        {
            tempstr += parseTime(score.time) + "\n";
        }
        return tempstr;
    }

    public static List<RacerScore> getTopLaptimes() => LTscores;

    public static void setToptimes(List<RacerScore> laptimes, List<RacerScore> totaltimes){
        LTscores = laptimes ?? throw new System.ArgumentNullException(nameof(laptimes));
        TTscores = totaltimes ?? throw new System.ArgumentNullException(nameof(totaltimes));
    }

    public static List<RacerScore> getTopTotaltimes() => TTscores;

    private static List<RacerScore> getList(string nameSelection, string trackSelection){
        if (nameSelection is null)
        {
            throw new System.ArgumentNullException(nameof(nameSelection));
        }

        List<RacerScore> items;
        switch(nameSelection){
            case "lap":
                items = sortstuff(LTscores, trackSelection);
                break;
            case "total":
                items = sortstuff(TTscores, trackSelection);
                break;
            default:
                items = sortstuff(LTscores, trackSelection);
                break;                
        }
        Debug.Log(items);
        return items;
    }

    private static string parseTime(float time){
        float secondsFloat = (float)time % 60;
        string seconds = secondsFloat.ToString("f2");
        if (secondsFloat <= 9){
            seconds = "0" + secondsFloat.ToString("f2");
        }
        string minutes = ((int)time / 60).ToString();
        string timerTime = minutes + ":" + seconds;
        return timerTime;
    }
    private static List<RacerScore> sortstuff(List<RacerScore> list, string trackSelection){
        if (list is null)
        {
            throw new System.ArgumentNullException(nameof(list));
        }
        var items = from score in list
                    where score.track == trackSelection
                    orderby score.time ascending
                    select score;
        foreach (RacerScore item in items){
            Debug.Log(item.racerName);
        }
        return items.ToList();
    }

    public static void newLapTime(string racerName,float time, string usedTrack){
        if (racerName is null)
        {
            throw new System.ArgumentNullException(nameof(racerName));
        }

        RacerScore newScore = new RacerScore(racerName,time,usedTrack); 
        //add new time to list
        LTscores.Add(newScore);
        //drop lowest
        //LTscores = sortstuff(LTscores, "ancient city");
        //LTscores.RemoveAt(9);
    }
    public static void newTotalTime(string racerName, float time, string usedTrack){
        if (racerName is null)
        {
            throw new System.ArgumentNullException(nameof(racerName));
        }

        RacerScore newScore = new RacerScore(racerName,time,usedTrack); 
        //add new time to list
        TTscores.Add(newScore);
        //drop lowest
        //TTscores = sortstuff(TTscores , "ancient city");
       // TTscores.RemoveAt(9);
    }
}
