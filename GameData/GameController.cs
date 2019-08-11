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
    private static bool musicSetting = true;
    private static string map = "Ancient City";

    public static string Difficulty { get => difficulty; set => difficulty = value; }
    public static string PlayerName { get => playerName; set => playerName = value; }
    public static string Mode { get => mode; set => mode = value; }
    public static string Map { get => map; set => map = value; }

    public void setSolo() => mode = "solo";
    public void setNormalMode() => mode = "normal";
    public void setEasyDif() => difficulty = "easy";
    public void setNormalDif() => difficulty = "normal";
    public void setHardDif() => difficulty = "hard";
    public void setMapDeepDesert() => map = "Deep Desert";
    public void setMapAncientCity() => map = "Ancient City";
    public void setMapGreatCliff() => map = "Great Cliff";
    public void setName(string arg0) => playerName = arg0;
    public static void setName2(string arg0) => playerName = arg0;
    public static void setMusic(bool on) => musicSetting = on;

    public static string getName() => playerName;
    public static bool getMusicSetting() => musicSetting;
    public static string getMap() => map;
    public static string getDifficulty() => difficulty;
    public static string getMode() => mode;

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.topLaptimes = HighScores.getTopLaptimes();
        save.topTotaltimes = HighScores.getTopTotaltimes();
        return save;
    }
    public void SaveGame()
    {
        Save save = CreateSaveGameObject();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Game Saved");
    }
    public void LoadGame()
    { 
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
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
