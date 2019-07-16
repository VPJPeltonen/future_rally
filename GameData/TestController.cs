using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public AIHover TestTarget;

    void Awake() => TestTarget.startRace();
}
