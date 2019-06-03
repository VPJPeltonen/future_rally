using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public AIHover TestTarget;
    // Start is called before the first frame update
    void Awake()
    {
        TestTarget.startRace();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
