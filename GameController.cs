using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
