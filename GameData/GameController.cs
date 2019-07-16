using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
public class GameController : MonoBehaviour
{
    private static string mode = "normal";
    private static string difficulty = "normal";
    private static string playerName = "PLAYER";

    public static string Difficulty { get => difficulty; set => difficulty = value; }
    public static string PlayerName { get => playerName; set => playerName = value; }
    public static string Mode { get => mode; set => mode = value; }

    public void setSolo() => mode = "solo";
    public void setNormalMode() => mode = "normal";
    public void setEasyDif() => difficulty = "easy";
    public void setNormalDif() => difficulty = "normal";
    public void setHardDif() => difficulty = "hard";
    public void setName(string arg0) => playerName = arg0;
    public static void setName2(string arg0) => playerName = arg0;
    public static string getName() => playerName;

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.topLaptimes = HighScores.getTopLaptimes();
        save.topTotaltimes = HighScores.getTopTotaltimes();
/* 
        int i = 0;
        foreach (GameObject targetGameObject in targets)
        {
            Target target = targetGameObject.GetComponent<Target>();
            if (target.activeRobot != null)
            {
            save.livingTargetPositions.Add(target.position);
            save.livingTargetsTypes.Add((int)target.activeRobot.GetComponent<Robot>().type);
            i++;
            }
        }

        save.hits = hits;
        save.shots = shots;
*/
        return save;
    }
    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
/* 
        // 3
        hits = 0;
        shots = 0;
        shotsText.text = "Shots: " + shots;
        hitsText.text = "Hits: " + hits;

        ClearRobots();
        ClearBullets();
        */
        Debug.Log("Game Saved");
    }
    public void LoadGame()
    { 
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            HighScores.setToptimes(save.topLaptimes, save.topTotaltimes);

        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
