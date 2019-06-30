using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static string mode = "solo";
    private static string difficulty = "normal";
    static public string playerName = "PLAYER";

    public static string Difficulty { get => difficulty; set => difficulty = value; }
    public static string Mode { get => mode; set => mode = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
